using System;
using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

using IdentityDemo.Infrastructure.Identity;

namespace SecureUserManagementDemo.Identity
{
   public class CustomUserClaimsPrincipalFactory
      : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
   {
      public CustomUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IOptions<IdentityOptions> options)
         : base(userManager, roleManager, options)
      {

      }

      protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
      {
         var id =  await base.GenerateClaimsAsync(user);

         id.AddClaim(new Claim("locale", user.Locale));

         return id;
      }
   }
}
