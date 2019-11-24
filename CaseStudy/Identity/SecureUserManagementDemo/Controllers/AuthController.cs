﻿using System;
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

            if (signInResult.Succeeded)
            {
               return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid UserName or Password");
         }

         return View();
      }

      #endregion

      #region Sign Out

      [HttpGet]
      public async Task<IActionResult> Logout()
      {
         if (User.Identity.IsAuthenticated)
         {
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


      #region Helper Methods

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

         if (user != null && await _userManager.CheckPasswordAsync(user, password))
         {
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
               ModelState.AddModelError("", "Email is not confirmed!");
               return Microsoft.AspNetCore.Identity.SignInResult.Failed;
            }

            ClaimsPrincipal principal = await _claimsPrincipalFactory.CreateAsync(user);

            await HttpContext.SignInAsync(scheme: IdentityConstants.ApplicationScheme, principal: principal);

            return Microsoft.AspNetCore.Identity.SignInResult.Success;
         }

         return Microsoft.AspNetCore.Identity.SignInResult.Failed;
      }

      #endregion
   }
}