using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace Chatting.API.Compositions
{
   public static class WebAPICompositions
   {
      public static IServiceCollection AddWebAPI(this IServiceCollection services, IHostingEnvironment environment)
      {
         services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

         //if(environment.IsLocal())
         //{
         //   // attach RequireHttps attribute (for all request) to Mvc Layer
         //   services.Configure<MvcOptions>(options =>
         //   {
         //      options.Filters.Add(new RequireHttpsAttribute());
         //   });
         //}

         return services;
      }
   }
}
