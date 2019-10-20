using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OdeToFood.Core.Interfaces;
using OdeToFood.Core.Models;

namespace OdeToFood.Pages.Restaurants
{
   public class ListModel : PageModel
   {
      private readonly IRestaurantRepository _restaurantRepository;

      public IEnumerable<Restaurant> Restaurants { get; private set; }

      public ListModel(IRestaurantRepository restaurantRepository)
      {
         _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
      }

      public async Task OnGet()
      {
         Restaurants = await _restaurantRepository.GetAllAsync();
      }
   }
}