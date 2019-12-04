using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using IdentityDemo.Infrastructure.Identity;

using LDTSolutions.Common.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SecureUserManagementDemo.Identity
{
   public static class IdentityServiceCollectionExtension
   {
      public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
      {
         //var connectionString = "Server=TRUNG-LUU\\TRUNGSQLSERVER;Database=PluralsightDemo;User ID=sa;Password=Trung1997;";
         var connectionString = "Server=localhost,1433;Database=PluralsightDemo;User ID=sa;Password=Trung1997;";
         var migrationAssembly = typeof(AppIdentityDbContext).Assembly.GetName().Name;
         services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(connectionString, opts => opts.MigrationsAssembly(migrationAssembly)));

         // regist BCryptPasswordHasher before the call AddIdentity to skip the default implementation, PasswordHasher<>
         services.AddScoped<IPasswordHasher<ApplicationUser>, BCryptPasswordHasher<ApplicationUser>>();

         services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
               options.Tokens.EmailConfirmationTokenProvider = CustomIdentityOptions.EmailConfirmationTokenProvider;

               options.Password.RequireNonAlphanumeric = true;
               options.Password.RequiredUniqueChars = 4;

               options.User.RequireUniqueEmail = true;

               options.Lockout.AllowedForNewUsers = true;
               options.Lockout.MaxFailedAccessAttempts = 3;
               options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, AppIdentityDbContext, Guid>>()
            .AddRoleStore<RoleStore<ApplicationRole, AppIdentityDbContext, Guid>>()
            .AddPasswordValidator<DoesNotContainPasswordValidator<ApplicationUser>>()
            .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<EmailConfirmationTokenProvider<ApplicationUser>>(CustomIdentityOptions.EmailConfirmationTokenProvider);

         services.Configure<DataProtectionTokenProviderOptions>(options =>
            options.TokenLifespan = TimeSpan.FromHours(1));

         services.Configure<EmailConfirmationTokenProviderOptions>(options =>
            options.TokenLifespan = TimeSpan.FromDays(2));

         services.ConfigureApplicationCookie(options =>
         {
            options.LoginPath = "/Auth/Login";
            options.Events = new CookieAuthenticationEvents
            {
               OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
            };
         });

         services.AddAuthentication()
            .AddGoogle("google", options =>
            {
               options.ClientId = "";
               options.ClientSecret = "";
               options.SignInScheme = IdentityConstants.ExternalScheme;
            });

         return services;
      }
   }
}
