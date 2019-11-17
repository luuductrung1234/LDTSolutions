using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SecureUserManagementDemo.Models;
using IdentityDemo.Infrastructure.Identity;

namespace SecureUserManagementDemo.Controllers
{
   public class HomeController : Controller
   {
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsPrincipalFactory;
      private readonly SignInManager<ApplicationUser> _signInManager;

      public HomeController(UserManager<ApplicationUser> userManager,
                              IUserClaimsPrincipalFactory<ApplicationUser> claimsPrincipalFactory,
                              SignInManager<ApplicationUser> signInManager)
      {
         _userManager = userManager;
         _claimsPrincipalFactory = claimsPrincipalFactory;
         _signInManager = signInManager;
      }

      public IActionResult Index()
      {
         return View();
      }

      [Authorize]
      public IActionResult Privacy()
      {
         return View();
      }

      [Authorize]
      public IActionResult About()
      {
         return View();
      }

      [HttpGet]
      public IActionResult Register()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Register(RegisterModel model)
      {
         if (ModelState.IsValid)
         {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
               user = new ApplicationUser()
               {
                  Id = Guid.NewGuid(),
                  UserName = model.UserName
               };

               var result = await _userManager.CreateAsync(user, model.Password);

               if (result.Errors.Count() == 0)
                  return View("Success");
               else
               {
                  foreach (var error in result.Errors)
                  {
                     ModelState.AddModelError("", error.Description);
                  }

                  return View();
               }
            }

            ModelState.AddModelError("", "Account is already exist!");
         }


         return View();
      }

      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
      public IActionResult Error()
      {
         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }

      [HttpGet]
      public IActionResult Login()
      {
         return View();
      }

      [HttpPost]
      public async Task<IActionResult> Login(LoginModel model)
      {
         if (ModelState.IsValid)
         {
            // var signInResult = await ManualPasswordSignInAsync(model.UserName, model.Password);

            // -------
            // SignInManager obscures too much of the authencation logic and furthur blurs the line between the UserStore and Authentication.
            //
            // (*) if you don't have to for authentication and user management -> use SignInManager
            // SignInManager also support:
            // - logout
            // - two factor
            //
            // (*) if you do have time, stick with the UserManager
            // -------
            var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (signInResult.Succeeded)
            {
               return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Invalid UserName or Password");
         }

         return View();
      }


      #region Helper Methods

      /// <summary>
      ///
      /// Manual implementation sign-in with password.
      ///
      /// Alternatively, use <see cref="SignInManager{TUser}.PasswordSignInAsync(string, string, bool, bool)"/>
      ///
      /// </summary>
      /// <param name="userName"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      private async Task<Microsoft.AspNetCore.Identity.SignInResult> ManualPasswordSignInAsync(string userName, string password)
      {
         var user = await _userManager.FindByNameAsync(userName);

         if (user != null && await _userManager.CheckPasswordAsync(user, password))
         {
            ClaimsPrincipal principal = await _claimsPrincipalFactory.CreateAsync(user);

            await HttpContext.SignInAsync(scheme: IdentityConstants.ApplicationScheme, principal: principal);

            return Microsoft.AspNetCore.Identity.SignInResult.Success;
         }

         return Microsoft.AspNetCore.Identity.SignInResult.Failed;
      }

      #endregion
   }
}
