using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;

// Infrastructure
using Identity.Infrastructure.Data;

namespace Identity.DummyAPI
{
   public class Startup
   {
      public IConfiguration Configuration { get; }

      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      // This method gets called by the runtime. Use this method to add services to the container.
      // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
      public void ConfigureServices(IServiceCollection services)
      {
         string connectionString = Configuration["IdentityDb:ConnectionString"];
         if (string.IsNullOrEmpty(connectionString))
         {
            throw new ArgumentException("IdentityDb connection string is null or empty!");
         }

         // Setup database
         services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(connectionString,
               sqlServerOptionsAction: sqlOptions =>
               {
                  //sqlOptions.MigrationsAssembly(typeof(AppIdentityDbContext).GetTypeInfo().Assembly.GetName().Name);
                  sqlOptions.MigrationsAssembly("Identity.Migrations");

                  //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                  sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
               }));
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.Run(async (context) =>
         {
            await context.Response.WriteAsync($"Dummy API as a startup-project for {typeof(AppIdentityDbContext).Name} migrations!");
         });
      }
   }
}
