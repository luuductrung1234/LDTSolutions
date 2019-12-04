using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SecureUserManagementDemo.Models;

using IdentityDemo.Infrastructure.Identity;
using System.Collections.Generic;

namespace SecureUserManagementDemo.Controllers
{
   public class AuthController : Controller
   {
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsPrincipalFactory;
      private readonly SignInManager<ApplicationUser> _signInManager;

      public AuthController(UserManager<ApplicationUser> userManager,
                              IUserClaimsPrincipalFactory<ApplicationUser> claimsPrincipalFactory,
                              SignInManager<ApplicationUser> signInManager)
      {
         _userManager = userManager;
         _claimsPrincipalFactory = claimsPrincipalFactory;
         _signInManager = signInManager;
      }

      [HttpGet]
      [Authorize]
      public async Task<IActionResult> Profiles()
      {
         var user = await _userManager.FindByNameAsync(this.User.Identity.Name);

         return View(new ProfilesModel()
         {
            UserName = user.UserName,
            Email = user.Email
         });
      }

      [HttpGet]
      [Authorize]
      public IActionResult Principal()
      {
         return View();
      }

      #region Register

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
                  UserName = model.UserName,
                  Email = model.Email
               };

               var result = await _userManager.CreateAsync(user, model.Password);

               if (result.Succeeded)
               {
                  var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                  // generate a confirm email url
                  var confirmtionEmailUrl = Url.Action("ConfirmEmailAddress", "Auth",
                     new { token = token, email = user.Email }, Request.Scheme);

                  // TODO: send the email contains this generated url
                  System.IO.File.WriteAllText("GeneratedUrl/confirmationLink.txt", confirmtionEmailUrl);

                  return View("Success");
               }
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

      #endregion

      #region Confirm Email

      [HttpGet]
      public async Task<IActionResult> ConfirmEmailAddress(string token, string email)
      {
         var user = await _userManager.FindByEmailAsync(email);
         if (user != null)
         {
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
               return View("Success");
            }
         }

         return View("Error");
      }

      #endregion

      #region Sign In

      [HttpGet]
      public IActionResult Login()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Login(LoginModel model)
      {
         if (ModelState.IsValid)
         {
            var signInResult = await CustomPasswordSignInAsync(model.UserName, model.Password);

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
            //var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (signInResult.RequiresTwoFactor)
            {
               return RedirectToAction("TwoFactor");
            }

            if (signInResult.Succeeded)
            {
               return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid UserName or Password");
         }

         return View();
      }

      /// <summary>
      ///
      /// Custom implementation sign-in with password.
      ///
      /// Alternatively, use <see cref="SignInManager{TUser}.PasswordSignInAsync(string, string, bool, bool)"/>
      ///
      /// </summary>
      /// <param name="userName"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      private async Task<Microsoft.AspNetCore.Identity.SignInResult> CustomPasswordSignInAsync(string userName, string password)
      {
         var user = await _userManager.FindByNameAsync(userName);

         if (user != null && !await _userManager.IsLockedOutAsync(user))
         {
            if (await _userManager.CheckPasswordAsync(user, password))
            {
               if (!await _userManager.IsEmailConfirmedAsync(user))
               {
                  ModelState.AddModelError("", "Email is not confirmed!");
                  return Microsoft.AspNetCore.Identity.SignInResult.Failed;
               }

               await _userManager.ResetAccessFailedCountAsync(user);

               if (await _userManager.GetTwoFactorEnabledAsync(user))
               {
                  // perform Two-step Verification

                  var validTFProviders = await _userManager.GetValidTwoFactorProvidersAsync(user);

                  if (validTFProviders.Contains(_userManager.Options.Tokens.AuthenticatorTokenProvider))
                  {
                     // Multi-Factor with local device which is a token generator (mobile app such as Google Authenticator, or Authy,...)

                     await HttpContext.SignInAsync(scheme: IdentityConstants.TwoFactorUserIdScheme,
                                                   principal: Store2FA(user.Id, _userManager.Options.Tokens.AuthenticatorTokenProvider));

                     return Microsoft.AspNetCore.Identity.SignInResult.TwoFactorRequired;
                  }

                  if (validTFProviders.Contains(TokenOptions.DefaultEmailProvider))
                  {
                     // Multi-Factor with generated token and send to user's email

                     var token = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);

                     // TODO: send the email contains this generated token
                     System.IO.File.WriteAllText("GeneratedUrl/email2sv.txt", token);

                     await HttpContext.SignInAsync(scheme: IdentityConstants.TwoFactorUserIdScheme,
                                                   principal: Store2FA(user.Id, TokenOptions.DefaultEmailProvider));

                     return Microsoft.AspNetCore.Identity.SignInResult.TwoFactorRequired;
                  }
               }

               ClaimsPrincipal principal = await _claimsPrincipalFactory.CreateAsync(user);

               await HttpContext.SignInAsync(scheme: IdentityConstants.ApplicationScheme,
                                             principal: principal);

               return Microsoft.AspNetCore.Identity.SignInResult.Success;
            }

            await _userManager.AccessFailedAsync(user);

            if (await _userManager.IsLockedOutAsync(user))
            {
               // at this time, user's account is locked out
               // email user, notifying them of lockout
            }
         }

         return Microsoft.AspNetCore.Identity.SignInResult.Failed;
      }

      private ClaimsPrincipal Store2FA(Guid userId, string provider)
      {
         var identity = new ClaimsIdentity(new List<Claim>
         {
            new Claim("sub", userId.ToString()),
            new Claim("amr", provider)
         }, IdentityConstants.TwoFactorUserIdScheme);

         return new ClaimsPrincipal(identity);
      }

      #endregion

      #region Two Factor

      [HttpGet]
      public IActionResult TwoFactor()
      {
         return View();
      }

      [HttpPost]
      public async Task<IActionResult> TwoFactor(TwoFactorModel model)
      {
         var result = await HttpContext.AuthenticateAsync(IdentityConstants.TwoFactorUserIdScheme);
         if (!result.Succeeded)
         {
            ModelState.AddModelError("", "You login request has expired, please start over.");

            return View();
         }

         if (ModelState.IsValid)
         {
            var userId = result.Principal.FindFirstValue("sub");

            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
               var provider = result.Principal.FindFirstValue("amr");

               var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, provider, model.Token);

               if (isValid)
               {
                  await HttpContext.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);

                  ClaimsPrincipal principal = await _claimsPrincipalFactory.CreateAsync(user);

                  await HttpContext.SignInAsync(scheme: IdentityConstants.ApplicationScheme,
                                                principal: principal);

                  return RedirectToAction("Index", "Home");
               }

               ModelState.AddModelError("", "Invalid token.");

               return View();
            }

            ModelState.AddModelError("", "Invalid token.");
         }

         return View();
      }

      [HttpGet]
      [Authorize]
      public async Task<IActionResult> RegisterAuthenticator()
      {
         var user = await _userManager.GetUserAsync(this.User);

         var authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);
         if (authenticatorKey == null)
         {
            await _userManager.ResetAuthenticatorKeyAsync(user);

            authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);
         }

         return View(new RegisterAuthenticatorModel { AuthenticatorKey = authenticatorKey });
      }

      [HttpGet]
      [Authorize]
      public async Task<IActionResult> RegisterAuthenticator(RegisterAuthenticatorModel model)
      {
         var user = await _userManager.GetUserAsync(this.User);

         var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, model.Code);

         if (!isValid)
         {
            ModelState.AddModelError("", "Code is invalid");

            return View(model);
         }

         await _userManager.SetTwoFactorEnabledAsync(user, true);

         return View("Success");
      }

      #endregion

      #region External Authentication Sign In

      public IActionResult ExternalLogin(string provider)
      {
         var properties = new AuthenticationProperties
         {
            RedirectUri = Url.Action("ExternalLoginCallback"),
            Items = { { "scheme", provider } }
         };

         return Challenge(properties, provider);
      }

      public async Task<IActionResult> ExternalLoginCallBack()
      {
         var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

         var externalUserId = result.Principal.FindFirstValue("sub")
                              ?? result.Principal.FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? throw new Exception("Cannot find external user id");

         var provider = result.Properties.Items["scheme"];

         var user = await _userManager.FindByLoginAsync(provider, externalUserId);

         if (user == null)
         {
            var email = result.Principal.FindFirstValue("email")
                        ?? result.Principal.FindFirstValue(ClaimTypes.Email);
            if (email != null)
            {
               user = await _userManager.FindByEmailAsync(email);

               if (user == null)
               {
                  user = new ApplicationUser() { UserName = email, Email = email };
                  await _userManager.CreateAsync(user);
               }

               await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, externalUserId, provider));
            }
         }

         if (user == null)
         {
            return View("Error");
         }

         await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

         var claimsPrincipal = await _claimsPrincipalFactory.CreateAsync(user);
         await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, claimsPrincipal);

         return RedirectToAction("Index");
      }

      #endregion

      #region Sign Out

      [HttpGet]
      public async Task<IActionResult> Logout()
      {
         if (User.Identity.IsAuthenticated)
         {
            // this method will signout all signin scheme
            // ApplicationScheme, ExternalScheme, TwoFactorUserScheme
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
         }

         return View("Error");
      }

      #endregion

      #region Forgot Password

      [HttpGet]
      public IActionResult ForgotPassword()
      {
         return View();
      }

      [HttpPost]
      public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
      {
         if (ModelState.IsValid)
         {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
               var token = await _userManager.GeneratePasswordResetTokenAsync(user);

               // generate request password url
               var requestPassworkUrl = Url.Action("ResetPassword", "Auth",
                  new { token = token, email = user.Email }, Request.Scheme);

               // TODO: send the email contains this generated url
               System.IO.File.WriteAllText("GeneratedUrl/resetLink.txt", requestPassworkUrl);

               return View("Success");
            }
            else
            {
               // email user and inform them that they do not have an account
               ModelState.AddModelError("Email", "An account with given email is not exist!");
            }

         }

         return View();
      }

      #endregion

      #region Reset Password

      [HttpGet]
      public IActionResult ResetPassword(string token, string email)
      {
         return View(new ResetPasswordModel() { Token = token, Email = email });
      }

      [HttpPost]
      public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
      {
         if (ModelState.IsValid)
         {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
               var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
               if (!result.Succeeded)
               {
                  foreach (var error in result.Errors)
                  {
                     ModelState.AddModelError("", error.Description);
                  }

                  return View();
               }

               if (await _userManager.IsLockedOutAsync(user))
               {
                  await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now);
               }

               return View("Success");
            }
         }

         ModelState.AddModelError("", "Invalid request!");

         return View();
      }

      #endregion

      #region Change Password

      [HttpPost]
      public async Task<IActionResult> ChangePassword(ProfilesModel model)
      {
         if (ModelState.IsValid)
         {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
               var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
               if (!result.Succeeded)
               {
                  foreach (var error in result.Errors)
                  {
                     ModelState.AddModelError("", error.Description);
                  }

                  return View("Profiles");
               }

               await _signInManager.SignOutAsync();

               return RedirectToAction("Index", "Home");
            }
         }

         ModelState.AddModelError("", "Invalid request!");

         return View("Profiles");
      }

      #endregion
   }
}