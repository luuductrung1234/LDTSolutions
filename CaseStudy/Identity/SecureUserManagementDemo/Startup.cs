using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using IdentityDemo.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace SecureUserManagementDemo
{
   public class Startup
   {
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.Configure<CookiePolicyOptions>(options =>
         {
               // This lambda determines whether user consent for non-essential cookies is needed for a given request.
               options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
         });


         services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

         //var connectionString = "Server=TRUNG-LUU\\TRUNGSQLSERVER;Database=PluralsightDemo;User ID=sa;Password=Trung1997;";
         var connectionString = "Server=localhost,1433;Database=PluralsightDemo;User ID=sa;Password=Trung1997;";
         var migrationAssembly = typeof(AppIdentityDbContext).Assembly.GetName().Name;
         services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(connectionString, opts => opts.MigrationsAssembly(migrationAssembly)));

         services.AddIdentity<ApplicationUser, ApplicationRole>(options => { })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, AppIdentityDbContext, Guid>>()
            .AddRoleStore<RoleStore<ApplicationRole, AppIdentityDbContext, Guid>>()
            .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>();

         services.ConfigureApplicationCookie(options =>
            options.LoginPath = "/Home/Login");
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
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
         }

         app.UseHttpsRedirection();

         app.UseAuthentication();

         app.UseStaticFiles();
         app.UseCookiePolicy();

         app.UseMvc(routes =>
         {
            routes.MapRoute(
                   name: "default",
                   template: "{controller=Home}/{action=Index}/{id?}");
         });
      }
   }
}
