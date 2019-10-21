using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OdeToFood.Core.Interfaces;
using OdeToFood.Core.Models;

namespace OdeToFood.Data.Repositories
{
   public class RestaurantRepository : IRestaurantRepository
   {
      public Task<Restaurant> AddAsync(Restaurant restaurant)
      {
         throw new NotImplementedException();
      }

      public int Commit()
      {
         throw new NotImplementedException();
      }

      public Task<IEnumerable<Restaurant>> GetAllAsync(string name)
      {
         throw new NotImplementedException();
      }

      public Task<Restaurant> GetByIdAsync(Guid id)
      {
         throw new NotImplementedException();
      }

      public Task<IEnumerable<Restaurant>> GetByNameAsync(string name)
      {
         throw new NotImplementedException();
      }

      public Task<Restaurant> UpdateAsync(Restaurant restaurant)
      {
         throw new NotImplementedException();
      }
   }
}
