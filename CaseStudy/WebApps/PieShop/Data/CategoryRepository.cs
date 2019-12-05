using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using PieShop.Models;

namespace PieShop.Data
{
   public class CategoryRepository : ICategoryRepository
   {
      private readonly AppDbContext _context;

      public CategoryRepository(AppDbContext context)
      {
         _context = context ?? throw new ArgumentNullException(nameof(context));
      }

      public async Task<IEnumerable<Category>> GetAllAsync()
      {
         return await _context.Categories.ToListAsync();
      }

      public async Task<Category> GetByIdAsync(int categoryId)
      {
         return await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
      }
   }
}
