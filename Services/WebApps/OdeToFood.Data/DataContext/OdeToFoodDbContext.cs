using Microsoft.EntityFrameworkCore;

using OdeToFood.Core.Models;

namespace OdeToFood.Data.DataContext
{
   public class OdeToFoodDbContext : DbContext
   {
      public DbSet<Restaurant> Restaurants { get; set; }
   }
}
