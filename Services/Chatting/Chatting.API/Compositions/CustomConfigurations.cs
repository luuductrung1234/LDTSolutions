using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

// Chatting Infrastructure
using Chatting.Infrastructure;

namespace Chatting.API.Compositions
{
   public static class CustomConfigurations
   {
      public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration Configuration)
      {
         services.AddOptions();

         services.Configure<DatabaseSetting>((options) =>
         {
            options.ConnectionString = Configuration["ServiceDb:ConnectionString"];
            options.DatabaseName = Configuration["ServiceDb:DatabaseName"];
         })
         .Configure<ReadOnlyDatabaseSetting>((options) =>
         {
            options.ConnectionString = Configuration["ServiceDb:ConnectionString"];
            options.DatabaseName = Configuration["ServiceDb:DatabaseName"];
         })
         .Configure<MasterDatabaseSetting>((options) =>
         {
            options.ConnectionString = Configuration["MasterDataDb:ConnectionString"];
         });

         return services;
      }
   }
}
