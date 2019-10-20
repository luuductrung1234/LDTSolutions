using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OdeToFood.Core.Interfaces;
using OdeToFood.Core.Models;

namespace OdeToFood.Data.Repositories
{
   public class RestaurantRepository : IRestaurantRepository
   {
      public Task<IEnumerable<Restaurant>> GetAllAsync()
      {
         throw new NotImplementedException();
      }
   }
}
