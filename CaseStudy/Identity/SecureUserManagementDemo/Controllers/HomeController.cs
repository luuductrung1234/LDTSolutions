using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using SecureUserManagementDemo.Models;

namespace SecureUserManagementDemo.Controllers
{
   public class HomeController : Controller
   {
      public HomeController()
      {
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
      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
      public IActionResult Error()
      {
         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }
   }
}
