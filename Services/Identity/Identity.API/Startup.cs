using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Autofac;

using SalesHub.Common.WebAPI;

// Identity API
using Identity.API.Compositions;
using Identity.API.Services;
using Identity.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Identity.API
{
   public class Startup
   {
      public Startup(IConfiguration configuration, IHostingEnvironment environment)
      {
         Configuration = configuration;
         Environment = environment;
      }

      public IHostingEnvironment Environment { get; }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public IServiceProvider ConfigureServices(IServiceCollection services)
      {
         services
            .AddCustomIdentity(Configuration)
            .AddWebAPI(Environment);

         services.AddTransient<ILoginService<ApplicationUser>, EFLoginService>();

         var container = new ContainerBuilder();
         container.Populate(services);

         return new AutofacServiceProvider(container.Build());
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
      {
         if (env.IsDevelopment() || env.IsLocal())
         {
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
         }
         else
         {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseExceptionHandler("/Home/Error");
         }

         var pathBase = Configuration["PATH_BASE"];
         if (!string.IsNullOrEmpty(pathBase))
         {
            loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
            app.UsePathBase(pathBase);
         }

         app.UseStaticFiles();

         app.UseAuthentication();

         app.UseHttpsRedirection();
         app.UseMvc();
      }
   }
}
