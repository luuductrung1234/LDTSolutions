using System;
using System.ComponentModel.DataAnnotations;

using OdeToFood.Core.Models;

namespace OdeToFood.Dtos
{
   public class EditRestaurantDto
   {
      public Guid Id { get; set; }

      [Required, StringLength(80)]
      public string Name { get; set; }

      [Required, StringLength(255)]
      public string Location { get; set; }

      public CuisineType CuisineType { get; set; }
   }
}
