using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using OdeToFood.Core.Interfaces;
using OdeToFood.Core.Models;

namespace OdeToFood.Pages.Restaurants
{
   public class EditModel : PageModel
   {
      private readonly IRestaurantRepository _restaurantRepository;
      private readonly IHtmlHelper _htmlHelper;

      [BindProperty]
      public Restaurant Restaurant { get; private set; }

      public IEnumerable<SelectListItem> Cuisines { get; private set; }

      public EditModel(IRestaurantRepository restaurantRepository,
                        IHtmlHelper htmlHelper)
      {
         _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
         _htmlHelper = htmlHelper ?? throw new ArgumentNullException(nameof(htmlHelper));
      }

      public async Task<IActionResult> OnGet(Guid restaurantId)
      {
         Cuisines = _htmlHelper.GetEnumSelectList<CuisineType>();

         Restaurant = await _restaurantRepository.GetByIdAsync(id: restaurantId);

         if(Restaurant == null)
         {
            return RedirectToPage("./NotFound");
         }

         return Page();
      }

      public async Task<IActionResult> OnPost()
      {
         Restaurant = await _restaurantRepository.UpdateAsync(Restaurant);
         _restaurantRepository.Commit();

         return Page();
      }
   }
}