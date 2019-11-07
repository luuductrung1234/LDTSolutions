using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdeToFood.Core.Models;

namespace OdeToFood.ViewModels
{
   public class RestaurantCountViewModel
   {
      public int Count { get; set; }

      public CuisineType? CuisineType { get; set; }
   }
}
