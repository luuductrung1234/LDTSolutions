using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OdeToFood.Core.Models;

namespace OdeToFood.Core.Interfaces
{
   public interface IRestaurantRepository
   {
      Task<IEnumerable<Restaurant>> GetAllAsync(string name);

      Task<IEnumerable<Restaurant>> GetByNameAsync(string name);

      Task<Restaurant> GetByIdAsync(Guid id);

      Task<Restaurant> UpdateAsync(Restaurant restaurant);

      int Commit();
   }
}
