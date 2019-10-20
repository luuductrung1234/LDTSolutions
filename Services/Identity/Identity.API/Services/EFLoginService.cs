using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

using Identity.Infrastructure.Data;

namespace Identity.API.Services
{
   public class EFLoginService : ILoginService<ApplicationUser>
   {
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly SignInManager<ApplicationUser> _signInManager;

      public EFLoginService(
         UserManager<ApplicationUser> userManager,
         SignInManager<ApplicationUser> signInManager)
      {
         _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
         _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
      }

      public async Task<ApplicationUser> FindByUserName(string userName)
      {
         return await _userManager.FindByEmailAsync(userName);
      }

      public async Task<bool> ValidateCredentials(ApplicationUser user, string password)
      {
         return await _userManager.CheckPasswordAsync(user, password);
      }

      public Task SignInAsync(ApplicationUser user)
      {
         return _signInManager.SignInAsync(user, true);
      }

      public Task SignInAsync(ApplicationUser user, AuthenticationProperties properties, string authenticationMethod = null)
      {
         return _signInManager.SignInAsync(user, properties, authenticationMethod);
      }
   }
}
