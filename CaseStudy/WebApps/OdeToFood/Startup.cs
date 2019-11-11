using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using LDTSolutions.Common.WebApi.NodeModules;
using OdeToFood.Data.Compositions;

namespace OdeToFood
{
   public class Startup
   {
      public Startup(IConfiguration configuration, IHostingEnvironment env)
      {
         Configuration = configuration;
         Environment = env;
      }

      public IConfiguration Configuration { get; }
      public IHostingEnvironment Environment { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         using (var scope = (new DefaultServiceProviderFactory()
                                 .CreateServiceProvider(services))
                                 .CreateScope())
         {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
            logger.LogInformation($"Application Service hosted in {Environment.EnvironmentName} environment");
            logger.LogInformation($"Web root path: {Environment.WebRootPath}");
         }

         services.Configure<CookiePolicyOptions>(options =>
         {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
         });

         services.AddDataLayer(configuration: Configuration);

         services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         else
         {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
         }

         app.UseHttpsRedirection();

         app.UseStaticFiles();

         // custom Middleware to provide client_side libraries in directory "~/node_modules/"
         app.UseNodeModules();

         app.UseCookiePolicy();

         app.UseMvc();
      }
   }
}
