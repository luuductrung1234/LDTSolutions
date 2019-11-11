using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;

using OdeToFood.Data.DataContext;

namespace OdeToFood
{
   public class Program
   {
      public static void Main(string[] args)
      {
         IWebHost host = CreateWebHostBuilder(args).Build();

         var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
         Directory.SetCurrentDirectory(path);

         AutoMigrateData(host);

         host.Run();
      }

      private static void AutoMigrateData(IWebHost host)
      {
         using (var scope = host.Services.CreateScope())
         {
            var dbContext = scope.ServiceProvider.GetRequiredService<OdeToFoodDbContext>();

            dbContext.Database.Migrate();
         }
      }

      public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
          WebHost.CreateDefaultBuilder(args)
              .UseStartup<Startup>();
   }
}
