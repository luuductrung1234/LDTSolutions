using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using OdeToFood.Core.Interfaces;
using OdeToFood.Core.Models;
using System.Threading;
using OdeToFood.Core.Exceptions;

namespace OdeToFood.Data.Repositories
{
   public class InMemoryRestaurantRepository : IRestaurantRepository
   {
      public List<Restaurant> _restaurants;

      public InMemoryRestaurantRepository()
      {
         _restaurants = new List<Restaurant>()
         {
            new Restaurant(id: Guid.NewGuid(), name: "Trung's Pizza", location: "Ho Chi Minh city", cuisineType: CuisineType.Italian),
            new Restaurant(id: Guid.NewGuid(), name: "Texas Chicken", location: "Texas", cuisineType: CuisineType.American),
            new Restaurant(id: Guid.NewGuid(), name: "Cari Home", location: "Banglades", cuisineType: CuisineType.Indian),
            new Restaurant(id: Guid.NewGuid(), name: "Donut", location: "Ho Chi Minh city", cuisineType: CuisineType.American)
         };
      }

      public async Task<IEnumerable<Restaurant>> GetAllAsync(string name = "", CancellationToken cancellationToken = default)
      {
         var query = _restaurants.AsQueryable();

         if (!string.IsNullOrEmpty(name))
         {
            query = query.Where(r => r.Name.StartsWith(name));
         }

         return await Task.FromResult(query.ToList());
      }

      public async Task<Restaurant> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
      {
         return await Task.FromResult(_restaurants.FirstOrDefault(r => r.Id == id));
      }

      public async Task<IEnumerable<Restaurant>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
      {
         return await Task.FromResult(_restaurants
            .Where(r => string.IsNullOrEmpty(name) || r.Name.StartsWith(name))
            .ToList());
      }

      public async Task<Restaurant> AddAsync(Restaurant restaurant, CancellationToken cancellationToken = default)
      {
         return await Task.Run(() =>
         {
            restaurant.GenerateId();

            _restaurants.Add(restaurant);

            return restaurant;
         }, cancellationToken);
      }

      public async Task<Restaurant> UpdateAsync(Restaurant restaurant, CancellationToken cancellationToken = default)
      {
         return await Task.Run(() =>
         {
            var restaurantToUpdate = _restaurants.FirstOrDefault(r => r.Id == restaurant.Id);
            if (restaurantToUpdate != null)
            {
               restaurantToUpdate.SetName(restaurant.Name);
               restaurantToUpdate.SetLocation(restaurant.Location);
               restaurantToUpdate.SetCuisineType(restaurant.CuisineType);
            }

            return restaurantToUpdate;
         }, cancellationToken);
      }

      public async Task<Restaurant> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
      {
         return await Task.Run(() =>
         {
            var restaurantToDelete = _restaurants.FirstOrDefault(r => r.Id == id);
            if (restaurantToDelete == null)
            {
               throw new RestaurantNotFoundException(id);
            }

            restaurantToDelete.SetDeleted();

            return restaurantToDelete;
         }, cancellationToken);
      }

      public async Task<int> CountAsync(CuisineType? cuisineType)
      {
         return await Task.Run(() =>
         {
            return _restaurants.Where(r => cuisineType.HasValue && r.CuisineType == cuisineType.Value).Count();
         });
      }
   }
}
