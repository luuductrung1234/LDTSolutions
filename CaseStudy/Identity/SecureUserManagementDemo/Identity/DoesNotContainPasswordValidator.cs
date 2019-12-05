using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SecureUserManagementDemo.Identity
{
   public class DoesNotContainPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : class
   {
      public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
      {
         var userName = await manager.GetUserNameAsync(user);

         if (userName == password)
            return IdentityResult.Failed(new IdentityError { Description = "Password contains UserName" });
         if (password.Contains(password))
            return IdentityResult.Failed(new IdentityError { Description = "Password contains the word \"password\"" });

         return IdentityResult.Success;
      }
   }
}
