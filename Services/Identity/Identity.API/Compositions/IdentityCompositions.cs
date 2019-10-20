using System;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI;

// Identity Infrastructure
using Identity.Infrastructure.Data;

namespace Identity.API.Compositions
{
   public static class IdentityCompositions
   {
      public static IServiceCollection AddCustomIdentity(this IServiceCollection services, IConfiguration configuration)
      {
         services.Configure<CookiePolicyOptions>(options =>
         {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
         });

         string connectionString = configuration["IdentityDb:ConnectionString"];
         if (string.IsNullOrEmpty(connectionString))
         {
            throw new ArgumentException("IdentityDb connection string is null or empty!");
         }

         // Setup database
         services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(connectionString,
               sqlServerOptionsAction: sqlOptions =>
               {
                  //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                  sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
               }));

         // Setup Identity services
         services.AddIdentity<ApplicationUser, IdentityRole>(options =>
               options.Password.RequireNonAlphanumeric = false)
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultUI(UIFramework.Bootstrap4)
            .AddDefaultTokenProviders();

         services.Configure<IdentityOptions>(options =>
         {
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;
         });

         services.ConfigureApplicationCookie(options =>
         {
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.SlidingExpiration = true;
         });

         return services;
      }
   }
}
