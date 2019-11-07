using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using OdeToFood.Core.Interfaces;
using OdeToFood.Core.Models;

namespace OdeToFood.Pages.Restaurants
{
   public enum ViewMode
   {
      Table,
      Grid
   }

   public class ListModel : PageModel
   {
      private readonly IRestaurantRepository _restaurantRepository;

      [TempData]
      public string Message { get; set; }

      public IEnumerable<Restaurant> Restaurants { get; private set; }

      public ViewMode ViewMode { get; private set; }

      [BindProperty(SupportsGet = true)]
      public string SearchTerm { get; set; }

      public ListModel(IRestaurantRepository restaurantRepository)
      {
         _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
      }

      public async Task<IActionResult> OnGet(ViewMode? viewMode)
      {
         Restaurants = await _restaurantRepository.GetAllAsync(name: SearchTerm);

         if (viewMode.HasValue)
         {
            ViewMode = viewMode.Value;
         }

         return Page();
      }
   }
}