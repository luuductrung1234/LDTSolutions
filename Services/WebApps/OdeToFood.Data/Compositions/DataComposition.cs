using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OdeToFood.Core.Interfaces;
using OdeToFood.Data.DataContext;
using OdeToFood.Data.Repositories;

namespace OdeToFood.Data.Compositions
{
   public static class DataComposition
   {
      public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
      {
         string connectionString = configuration["ConnectionStrings:OdeToFoodDb"];
         if (string.IsNullOrEmpty(connectionString))
         {
            throw new ArgumentException("Database connection string is not valid!");
         }

         services.AddDbContext<OdeToFoodDbContext>(options =>
            options.UseSqlServer(connectionString));

         services.AddScoped<IRestaurantRepository, RestaurantRepository>();

         return services;
      }
   }
}
