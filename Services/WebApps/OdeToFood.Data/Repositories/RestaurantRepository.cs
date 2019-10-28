using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OdeToFood.Core.Exceptions;
using OdeToFood.Core.Interfaces;
using OdeToFood.Core.Models;
using OdeToFood.Data.DataContext;

namespace OdeToFood.Data.Repositories
{
   public class RestaurantRepository : IRestaurantRepository
   {
      private readonly OdeToFoodDbContext _dbContext;

      public RestaurantRepository(OdeToFoodDbContext dbContext)
      {
         _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
      }

      public async Task<Restaurant> AddAsync(Restaurant restaurant, CancellationToken cancellationToken = default)
      {
         var addResult = await _dbContext.AddAsync(restaurant, cancellationToken);

         int effectedRows = await _dbContext.SaveChangesAsync();
         if (effectedRows == 0)
         {
            return null;
         }

         return addResult.Entity;
      }

      public async Task<IEnumerable<Restaurant>> GetAllAsync(string name, CancellationToken cancellationToken = default)
      {
         var query = _dbContext.Restaurants.Where(r => r.IsDeleted == false);

         if (!string.IsNullOrEmpty(name))
         {
            query = query.Where(r => r.Name.StartsWith(name));
         }

         return await query.ToListAsync(cancellationToken);
      }

      public async Task<Restaurant> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
      {
         return await _dbContext.Restaurants.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
      }

      public async Task<IEnumerable<Restaurant>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
      {
         return await _dbContext.Restaurants.Where(r => r.Name.Contains(name)).ToListAsync(cancellationToken);
      }

      public async Task<Restaurant> UpdateAsync(Restaurant restaurant, CancellationToken cancellationToken = default)
      {
         var restaurantToUpdate = await _dbContext.Restaurants.SingleOrDefaultAsync(r => r.Id == restaurant.Id);
         restaurantToUpdate.SetName(restaurant.Name);
         restaurantToUpdate.SetLocation(restaurant.Location);
         restaurantToUpdate.SetCuisineType(restaurant.CuisineType);

         var entity = _dbContext.Attach(restaurantToUpdate);
         entity.State = EntityState.Modified;

         do
         {
            try
            {
               // Attempt to save changes to the database
               int effectedRows = await _dbContext.SaveChangesAsync();
               if (effectedRows == 0)
               {
                  return null;
               }

               return restaurantToUpdate;
            }
            catch (DbUpdateConcurrencyException ex)
            {
               foreach (var entry in ex.Entries)
               {
                  if (entry.Entity is Restaurant)
                  {
                     var proposedValues = entry.CurrentValues;
                     var databaseValues = entry.GetDatabaseValues();

                     foreach (var property in proposedValues.Properties)
                     {
                        var proposedValue = proposedValues[property];
                        var databaseValue = databaseValues[property];

                        // TODO: decide which value should be written to database
                        // proposedValues[property] = <value to be saved>;
                     }

                     // Refresh original values to bypass next concurrency check
                     entry.OriginalValues.SetValues(databaseValues);
                  }
                  else
                  {
                     throw new NotSupportedException($"Don't know how to handle concurrency conflicts for {entry.Metadata.Name}");
                  }
               }
            }
         } while (true);
      }

      public async Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
      {
         var restaurantToUpdate = await _dbContext.Restaurants.SingleOrDefaultAsync(r => r.Id == id);
         if (restaurantToUpdate == null)
         {
            throw new RestaurantNotFoundException(id);
         }

         restaurantToUpdate.SetDeleted();

         var entity = _dbContext.Attach(restaurantToUpdate);
         entity.State = EntityState.Modified;

         do
         {
            try
            {
               // Attempt to save changes to the database
               return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
               foreach (var entry in ex.Entries)
               {
                  if (entry.Entity is Restaurant)
                  {
                     var proposedValues = entry.CurrentValues;
                     var databaseValues = entry.GetDatabaseValues();

                     foreach (var property in proposedValues.Properties)
                     {
                        var proposedValue = proposedValues[property];
                        var databaseValue = databaseValues[property];

                        // TODO: decide which value should be written to database
                        // proposedValues[property] = <value to be saved>;
                     }

                     // Refresh original values to bypass next concurrency check
                     entry.OriginalValues.SetValues(databaseValues);
                  }
                  else
                  {
                     throw new NotSupportedException($"Don't know how to handle concurrency conflicts for {entry.Metadata.Name}");
                  }
               }
            }
         } while (true);
      }
   }
}
