using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;

namespace LDTSolutions.Common.WebApi.NodeModules
{
   public static class ApplicationBuilderExtensions
   {
      public static IApplicationBuilder UseNodeModules(this IApplicationBuilder app,
                                                      TimeSpan? maxAge = null,
                                                      string requestPath = "/node_modules")
      {
         if (app == null) throw new ArgumentNullException(nameof(app));

         AddMiddleWare(app, maxAge, requestPath);

         return app;
      }

      private static void AddMiddleWare(IApplicationBuilder app, TimeSpan? maxAge, string requestPath)
      {
         var environment = (IHostingEnvironment)app.ApplicationServices.GetService(typeof(IHostingEnvironment));

         var path = Path.Combine(environment.ContentRootPath, "node_modules");
         var provider = new PhysicalFileProvider(path);

         var options = new FileServerOptions { RequestPath = requestPath };
         options.StaticFileOptions.FileProvider = provider;

         if(maxAge != null)
         {
            options.StaticFileOptions.OnPrepareResponse = context =>
            {
               var headers = context.Context.Response.GetTypedHeaders();
               headers.CacheControl = new CacheControlHeaderValue { MaxAge = maxAge, Public = true };
            };
         }

         options.EnableDirectoryBrowsing = false;

         app.UseFileServer(options);
      }
   }
}
