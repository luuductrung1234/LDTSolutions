using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IdentityDemo.Infrastructure.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Identity
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
