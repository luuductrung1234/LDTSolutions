using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Identity.API.Services
{
   public interface ILoginService<T>
   {
      Task<bool> ValidateCredentials(T user, string password);

      Task<T> FindByUserName(string userName);

      Task SignInAsync(T user);

      Task SignInAsync(T user, AuthenticationProperties properties, string authenticationMethod = null);
   }
}
