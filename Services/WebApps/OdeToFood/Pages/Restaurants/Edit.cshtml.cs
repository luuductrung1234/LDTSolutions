using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using OdeToFood.Core.Interfaces;
using OdeToFood.Core.Models;
using OdeToFood.Dtos;

namespace OdeToFood.Pages.Restaurants
{
   public class EditModel : PageModel
   {
      private readonly IRestaurantRepository _restaurantRepository;
      private readonly IHtmlHelper _htmlHelper;

      public string Message { get; set; }

      [BindProperty]
      public EditRestaurantDto RestaurantDto { get; set; }

      public IEnumerable<SelectListItem> Cuisines { get; private set; }

      public EditModel(IRestaurantRepository restaurantRepository,
                        IHtmlHelper htmlHelper)
      {
         _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
         _htmlHelper = htmlHelper ?? throw new ArgumentNullException(nameof(htmlHelper));
      }

      public async Task<IActionResult> OnGet(Guid? restaurantId)
      {
         Cuisines = _htmlHelper.GetEnumSelectList<CuisineType>();

         RestaurantDto = new EditRestaurantDto();

         if (restaurantId.HasValue && restaurantId.Value != Guid.Empty)
         {
            var fetchedRestaurant = await _restaurantRepository.GetByIdAsync(id: restaurantId.Value);
            if (RestaurantDto == null)
            {
               return RedirectToPage("./NotFound");
            }

            RestaurantDto.Id = fetchedRestaurant.Id;
            RestaurantDto.Name = fetchedRestaurant.Name;
            RestaurantDto.Location = fetchedRestaurant.Location;
            RestaurantDto.CuisineType = fetchedRestaurant.CuisineType;
         }

         return Page();
      }

      public async Task<IActionResult> OnPost()
      {
         Message = string.Empty;

         if (ModelState.IsValid)
         {
            if (RestaurantDto.Id != Guid.Empty)
            {
               var fetchedRestaurant = await _restaurantRepository.GetByIdAsync(id: RestaurantDto.Id);

               fetchedRestaurant.SetName(RestaurantDto.Name);
               fetchedRestaurant.SetLocation(RestaurantDto.Location);
               fetchedRestaurant.SetCuisineType(RestaurantDto.CuisineType);

               var updatedRestaurant = await _restaurantRepository.UpdateAsync(fetchedRestaurant);
               if (updatedRestaurant == null)
               {
                  Message = "Fail to update Restaurant! Please try again.";
               }
               else
               {
                  TempData["Message"] = "Restaurant updated!";

                  return RedirectToPage("./Detail", new { restaurantId = updatedRestaurant.Id });
               }
            }
            else
            {
               var newRestaurant = new Restaurant(name: RestaurantDto.Name,
                  location: RestaurantDto.Location,
                  cuisineType: RestaurantDto.CuisineType);

               var createdRestaurant = await _restaurantRepository.AddAsync(newRestaurant);

               if (createdRestaurant == null)
               {
                  Message = "Fail to create new Restaurant! Please try again.";
               }
               else
               {
                  TempData["Message"] = "Restaurant created!";

                  return RedirectToPage("./Detail", new { restaurantId = createdRestaurant.Id });
               }
            }

         }

         Cuisines = _htmlHelper.GetEnumSelectList<CuisineType>();

         return Page();
      }
   }
}