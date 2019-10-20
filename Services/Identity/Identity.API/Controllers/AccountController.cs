using System;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

using Identity.Infrastructure.Data;
using Identity.API.Services;
using Identity.API.Models.AccountViewModels;

namespace Identity.API.Controllers
{
   public class AccountController : Controller
   {
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly ILoginService<ApplicationUser> _loginService;
      private readonly ILogger<AccountController> _logger;
      private readonly IConfiguration _configuration;

      public AccountController(
         UserManager<ApplicationUser> userManager,
         ILoginService<ApplicationUser> loginService,
         ILogger<AccountController> logger,
         IConfiguration configuration)
      {
         _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
         _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
         _logger = logger ?? throw new ArgumentNullException(nameof(logger));
         _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
      }

      #region Register

      [HttpGet]
      [AllowAnonymous]
      public IActionResult Register(string returnUrl = null)
      {
         ViewData["ReturnUrl"] = returnUrl;

         return View();
      }

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = "")
      {
         if (ModelState.IsValid)
         {
            var user = new ApplicationUser
            {
               UserName = model.Email,
               Email = model.Email,
               CardHolderName = model.User.CardHolderName,
               CardNumber = model.User.CardNumber,
               CardType = model.User.CardType,
               City = model.User.City,
               Country = model.User.Country,
               Expiration = model.User.Expiration,
               LastName = model.User.LastName,
               Name = model.User.Name,
               Street = model.User.Street,
               State = model.User.State,
               ZipCode = model.User.ZipCode,
               PhoneNumber = model.User.PhoneNumber,
               SecurityNumber = model.User.SecurityNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Errors.Count() > 0)
            {
               foreach (var error in result.Errors)
               {
                  ModelState.AddModelError(string.Empty, error.Description);
               }

               return View(model);
            }
         }

         if (returnUrl != null)
         {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
               return Redirect(returnUrl);
            }
            else
            {
               if (ModelState.IsValid)
               {
                  return RedirectToAction("Login", "Account", new { returnUrl = returnUrl });
               }
               else
               {
                  return View(model);
               }
            }
         }

         ViewData["ReturnUrl"] = returnUrl;

         return RedirectToAction("Index", "Home");
      }

      #endregion

      #region Login

      /// <summary>
      /// Show login page
      /// </summary>
      [HttpGet]
      [AllowAnonymous]
      public async Task<IActionResult> Login(string returnUrl = null)
      {
         var vm = await BuildLoginViewModelAsync(returnUrl);

         ViewData["ReturnUrl"] = returnUrl;

         return View(vm);
      }

      /// <summary>
      /// Handle postback from username/password name
      /// </summary>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Login(LoginViewModel model)
      {
         if (ModelState.IsValid)
         {
            var user = await _loginService.FindByUserName(model.Email);

            if (await _loginService.ValidateCredentials(user, model.Password))
            {
               var tokenLifetime = _configuration.GetValue("Token:TokenLifeTimeMinutes", 120);

               var props = new AuthenticationProperties()
               {
                  ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(tokenLifetime),
                  AllowRefresh = true,
                  RedirectUri = model.ReturnUrl
               };

               if (model.RememberMe)
               {
                  var permanentTokenLifetime = _configuration.GetValue("Token:PermanentTokenLifetimeDays", 365);

                  props.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(permanentTokenLifetime);
                  props.IsPersistent = true;
               }

               await _loginService.SignInAsync(user, props);

               return Redirect("~/");
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password.");
         }

         // something when wrong, show form with errors
         var vm = await BuildLoginViewModelAsync(model);

         ViewData["ReturnUrl"] = model.ReturnUrl;

         return View(vm);
      }

      #endregion

      #region Logout

      [HttpGet]
      public async Task<IActionResult> Logout(string logoutId)
      {
         if (User.Identity.IsAuthenticated)
         {
            // if the user is not authenticated, then just show logged out page
            return await Logout(new LogoutViewModel() { LogoutId = logoutId });
         }

         // show the logout prompt. this prevents attacks where the user
         // is automatically signed out by another malicious web page.
         var vm = new LogoutViewModel
         {
            LogoutId = logoutId
         };
         return View(vm);
      }

      /// <summary>
      /// Handle logout page postback
      /// </summary>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Logout(LogoutViewModel model)
      {
         // delete authenticate cookie
         await HttpContext.SignOutAsync();

         await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

         // set this so UI rendering sees an anonymous user
         HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

         return RedirectToAction("Index", "Home");
      }

      #endregion

      #region Helper Methods

      private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
      {
         return await Task.FromResult(new LoginViewModel()
         {
            ReturnUrl = returnUrl,
            Email = ""
         });
      }

      private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel model)
      {
         var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
         vm.Email = model.Email;
         vm.RememberMe = model.RememberMe;
         return vm;
      }

      #endregion
   }
}