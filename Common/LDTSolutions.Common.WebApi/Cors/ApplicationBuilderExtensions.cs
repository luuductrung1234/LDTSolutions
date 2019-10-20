using System;
using Microsoft.AspNetCore.Builder;

namespace LDTSolutions.Common.WebApi.Cors
{
   public static class ApplicationBuilderExtensions
   {
      public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
      {
         app.UseCors(CorsConstants.CorsCustomPolicy);

         return app;
      }
   }
}
