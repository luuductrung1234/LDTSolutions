using Microsoft.Extensions.DependencyInjection;

namespace LDTSolutions.Common.WebApi.Cors
{
   public static class ServiceCollectionExtensions
   {
      public static IServiceCollection AddCustomCors(this IServiceCollection services)
      {
         services
            .AddCors(options =>
            {
               options.AddPolicy(CorsConstants.CorsCustomPolicy, builder =>
               {
                  builder
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials()
                     .AllowAnyOrigin()
                     .WithOrigins("http://localhost:8080");
               });
            });

         return services;
      }
   }
}
