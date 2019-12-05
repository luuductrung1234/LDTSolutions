using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using IdentityWithEFDemo.Models;
using IdentityDemo.Infrastructure.Identity;

namespace IdentityWithEFDemo.Controllers
{
   public class HomeController : Controller
   {
      private readonly UserManager<ApplicationUser> userManager;

      public HomeController(UserManager<ApplicationUser> userManager)
      {
         this.userManager = userManager;
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
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
               user = new ApplicationUser()
               {
                  Id = Guid.NewGuid(),
                  UserName = model.UserName
               };

               var result = await userManager.CreateAsync(user, model.Password);

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
            var user = await userManager.FindByNameAsync(model.UserName);

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
               var identity = new ClaimsIdentity("cookies");
               identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
               identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

               await HttpContext.SignInAsync("cookies", new ClaimsPrincipal(identity));

               return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Invalid UserName or Password");
         }

         return View();
      }
   }
}
