using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using OdeToFood.Core.Interfaces;
using OdeToFood.Core.Models;

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

      public async Task<IEnumerable<Restaurant>> GetAllAsync(string name)
      {
         var query = _restaurants.AsQueryable();

         if (!string.IsNullOrEmpty(name))
         {
            query = query.Where(r => r.Name.StartsWith(name));
         }

         return await Task.FromResult(query.ToList());
      }

      public async Task<Restaurant> GetByIdAsync(Guid id)
      {
         return await Task.FromResult(_restaurants.FirstOrDefault(r => r.Id == id));
      }

      public async Task<IEnumerable<Restaurant>> GetByNameAsync(string name)
      {
         return await Task.FromResult(_restaurants
            .Where(r => string.IsNullOrEmpty(name) || r.Name.StartsWith(name))
            .ToList());
      }

      public async Task<Restaurant> AddAsync(Restaurant restaurant)
      {
         return await Task.Run(() =>
         {
            restaurant.GenerateId();

            _restaurants.Add(restaurant);

            return restaurant;
         });
      }

      public async Task<Restaurant> UpdateAsync(Restaurant restaurant)
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
         });
      }

      public int Commit()
      {
         return 0;
      }
   }
}
