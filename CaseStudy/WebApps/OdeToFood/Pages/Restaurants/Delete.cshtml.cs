using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using OdeToFood.Core.Exceptions;
using OdeToFood.Core.Interfaces;
using OdeToFood.Core.Models;

namespace OdeToFood.Pages.Restaurants
{
   public class DeleteModel : PageModel
   {
      private readonly IRestaurantRepository _restaurantRepository;

      public Restaurant Restaurant { get; set; }

      public DeleteModel(IRestaurantRepository restaurantRepository)
      {
         _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
      }

      public async Task<IActionResult> OnGet(Guid restaurantId)
      {
         Restaurant = await _restaurantRepository.GetByIdAsync(restaurantId, CancellationToken.None);
         if (Restaurant == null)
         {
            return RedirectToPage("./NotFound");
         }

         return Page();
      }

      public async Task<IActionResult> OnPost(Guid restaurantId)
      {
         try
         {
            Restaurant deletedRestaurant = await _restaurantRepository.DeleteAsync(restaurantId, CancellationToken.None);
            if (deletedRestaurant == null)
            {
               return RedirectToPage("./Errors");
            }

            TempData["Message"] = $"{deletedRestaurant.Name} is deleted.";
            return RedirectToPage("./List");
         }
         catch (RestaurantNotFoundException)
         {
            return RedirectToPage("./NotFound");
         }
      }
   }
}