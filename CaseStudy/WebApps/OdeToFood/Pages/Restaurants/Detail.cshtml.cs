using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using OdeToFood.Core.Interfaces;
using OdeToFood.Core.Models;

namespace OdeToFood.Pages.Restaurants
{
   public class DetailModel : PageModel
   {
      private readonly IRestaurantRepository _restaurantRepository;

      [TempData]
      public string Message { get; set; }

      public Restaurant Restaurant { get; private set; }

      public DetailModel(IRestaurantRepository restaurantRepository)
      {
         _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
      }

      public async Task<IActionResult> OnGet(Guid restaurantId)
      {
         if(restaurantId == Guid.Empty)
         {
            return RedirectToPage("./NotFound");
         }

         Restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);

         return Page();
      }
   }
}