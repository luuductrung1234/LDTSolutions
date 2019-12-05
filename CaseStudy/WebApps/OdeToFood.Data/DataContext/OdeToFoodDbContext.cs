using Microsoft.EntityFrameworkCore;

using OdeToFood.Core.Models;
using OdeToFood.Data.EntityConfiguration;

namespace OdeToFood.Data.DataContext
{
   public class OdeToFoodDbContext : DbContext
   {
      public DbSet<Restaurant> Restaurants { get; set; }

      public OdeToFoodDbContext(DbContextOptions<OdeToFoodDbContext> options) : base(options)
      {

      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);

         // custom mapping configrations
         modelBuilder.ApplyConfiguration(new RestaurantEntityTypeConfiguration());
      }
   }
}
