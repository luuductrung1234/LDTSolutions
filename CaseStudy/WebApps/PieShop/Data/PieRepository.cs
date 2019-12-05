using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using PieShop.Models;

namespace PieShop.Data
{
   public class PieRepository : IPieRepository
   {
      private readonly AppDbContext _context;

      public PieRepository(AppDbContext context)
      {
         _context = context ?? throw new ArgumentNullException(nameof(context));
      }

      public async Task<IEnumerable<Pie>> GetAllAsync(decimal? fromPrice = null, decimal? toPrice = null, int? categoryId = null, bool? isPieOfTheWeeK = null)
      {
         var query = _context.Pies.AsQueryable();

         if (fromPrice.HasValue)
         {
            query = query.Where(p => p.Price >= fromPrice);
         }

         if (toPrice.HasValue)
         {
            query = query.Where(p => p.Price <= toPrice);
         }

         if (categoryId.HasValue)
         {
            query = query.Include(p => p.Category).Where(p => p.CategoryId == categoryId);
         }

         if (isPieOfTheWeeK.HasValue)
         {
            query = query.Where(p => p.IsPieOfTheWeek == isPieOfTheWeeK);
         }

         return await query.ToListAsync();
      }

      public async Task<Pie> GetByIdAsync(int pieId)
      {
         return await _context.Pies.FirstOrDefaultAsync(p => p.Id == pieId);
      }
   }
}
