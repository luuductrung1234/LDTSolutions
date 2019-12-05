using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using OdeToFood.Core.Models;

namespace OdeToFood.Core.Interfaces
{
   public interface IRestaurantRepository
   {
      Task<IEnumerable<Restaurant>> GetAllAsync(string name = "", CancellationToken cancellationToken = default);

      Task<IEnumerable<Restaurant>> GetByNameAsync(string name, CancellationToken cancellationToken = default);

      Task<Restaurant> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

      Task<int> CountAsync(CuisineType? cuisineType);

      Task<Restaurant> AddAsync(Restaurant restaurant, CancellationToken cancellationToken = default);

      Task<Restaurant> UpdateAsync(Restaurant restaurant, CancellationToken cancellationToken = default);

      Task<Restaurant> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
   }
}
