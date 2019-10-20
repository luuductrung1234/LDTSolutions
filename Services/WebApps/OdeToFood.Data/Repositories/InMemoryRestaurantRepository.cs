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

      public async Task<IEnumerable<Restaurant>> GetByNameAsync(string name)
      {
         return await Task.FromResult(_restaurants
            .Where(r => string.IsNullOrEmpty(name) || r.Name.StartsWith(name))
            .ToList());
      }
   }
}
