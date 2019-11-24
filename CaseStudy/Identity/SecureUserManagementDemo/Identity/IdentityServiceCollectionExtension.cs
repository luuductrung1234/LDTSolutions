using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using IdentityDemo.Infrastructure.Identity;

using LDTSolutions.Common.Identity;

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
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, AppIdentityDbContext, Guid>>()
            .AddRoleStore<RoleStore<ApplicationRole, AppIdentityDbContext, Guid>>()
            .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<EmailConfirmationTokenProvider<ApplicationUser>>(CustomIdentityOptions.EmailConfirmationTokenProvider);

         services.Configure<DataProtectionTokenProviderOptions>(options =>
            options.TokenLifespan = TimeSpan.FromHours(1));

         services.Configure<EmailConfirmationTokenProviderOptions>(options =>
            options.TokenLifespan = TimeSpan.FromDays(2));

         services.ConfigureApplicationCookie(options => options.LoginPath = "/Auth/Login");

         return services;
      }
   }
}
