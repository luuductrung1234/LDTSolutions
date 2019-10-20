using System.Collections.Generic;
using System.Threading.Tasks;

using PieShop.Models;

namespace PieShop.Data
{
   public interface ICategoryRepository
   {
      Task<IEnumerable<Category>> GetAllAsync();

      Task<Category> GetByIdAsync(int categoryId);
   }
}
