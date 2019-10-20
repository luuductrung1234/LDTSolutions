using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OdeToFood.Core.Interfaces;
using OdeToFood.Core.Models;

namespace OdeToFood.Data.Repositories
{
   public class RestaurantRepository : IRestaurantRepository
   {
      public Task<IEnumerable<Restaurant>> GetAllAsync(string name)
      {
         throw new NotImplementedException();
      }

      public Task<IEnumerable<Restaurant>> GetByNameAsync(string name)
      {
         throw new NotImplementedException();
      }
   }
}
