using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using OdeToFood.Core.Interfaces;
using OdeToFood.Core.Models;
using OdeToFood.ViewModels;

namespace OdeToFood.ViewComponents
{
   public class RestaurantCountViewComponent : ViewComponent
   {
      private readonly IRestaurantRepository _restaurantRepository;

      public RestaurantCountViewComponent(IRestaurantRepository restaurantRepository, IHtmlHelper htmlHelper)
      {
         _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
      }

      public async Task<IViewComponentResult> InvokeAsync(CuisineType? cuisineType)
      {
         var response = new RestaurantCountViewModel()
         {
            Count = await _restaurantRepository.CountAsync(cuisineType),
            CuisineType = cuisineType
         };

         return View(response);
      }
   }
}
