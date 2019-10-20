using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PieShop.Data;

namespace PieShop
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
         string connectionString = Configuration["MasterDb:ConnectionString"];
         services.AddDbContext<AppDbContext>(options =>
            {
               options.UseSqlServer(connectionString);
               options.EnableSensitiveDataLogging(true);
            });

         services.AddScoped<IPieRepository, PieRepository>();
         services.AddScoped<ICategoryRepository, CategoryRepository>();

         services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseHttpsRedirection();

         app.UseStaticFiles();

         app.UseMvcWithDefaultRoute();
      }
   }
}
