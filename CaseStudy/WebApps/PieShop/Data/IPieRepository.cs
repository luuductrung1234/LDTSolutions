using System.Collections.Generic;
using System.Threading.Tasks;

using PieShop.Models;

namespace PieShop.Data
{
   public interface IPieRepository
   {
      Task<IEnumerable<Pie>> GetAllAsync(decimal? fromPrice = null, decimal? toPrice = null, int? categoryId = null, bool? isPieOfTheWeeK = null);

      Task<Pie> GetByIdAsync(int pieId);
   }
}
