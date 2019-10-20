using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

using OdeToFood.Core.Interfaces;
using OdeToFood.Data.Repositories;

namespace OdeToFood.Data.Compositions
{
   public static class DataComposition
   {
      public static IServiceCollection AddDataLayer(this IServiceCollection services)
      {
         services.AddSingleton<IRestaurantRepository, InMemoryRestaurantRepository>();

         return services;
      }
   }
}
