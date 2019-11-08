using System;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

// Common
using LDTSolutions.Common.WebApi.Mvc;

// OdeToFood Core
using OdeToFood.Core.Interfaces;
using OdeToFood.Core.Models;

namespace OdeToFood.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class RestaurantsController : ControllerBase
   {
      private readonly IRestaurantRepository _restaurantRepository;

      public RestaurantsController(IRestaurantRepository restaurantRepository)
      {
         _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
      }

      [HttpGet]
      public async  Task<IActionResult> GetRestaurants()
      {
         IEnumerable<Restaurant> restaurants = await _restaurantRepository.GetAllAsync();

         return this.OkResult(restaurants);
      }

      [HttpGet("{id}")]
      public async  Task<IActionResult> GetRestaurant(Guid id)
      {
         Restaurant restaurant = await _restaurantRepository.GetByIdAsync(id);
         if (restaurant == null)
            return this.ErrorResult("", $"Restaurant with id:{id} is not found!", HttpStatusCode.NotFound);

         return  this.OkResult(restaurant);
      }

      [HttpPost]
      public async Task<IActionResult> PostRestaurant([FromBody] Restaurant restaurant)
      {
         if (!ModelState.IsValid)
            return this.ErrorResult("", "HttpPost data is not valid!", HttpStatusCode.BadRequest);

         Restaurant createdRestaurant =  await _restaurantRepository.AddAsync(restaurant);
         if(createdRestaurant == null)
            return this.ErrorResult("", "Fail to create restaurant!");

         return CreatedAtAction(nameof(GetRestaurant), new { id = createdRestaurant.Id });
      }

      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteRestaurant(Guid id)
      {
         var deletedRestaurant = await _restaurantRepository.DeleteAsync(id);
         if(deletedRestaurant == null)
            return this.ErrorResult("", "Fail to delete restaurant!");

         return this.OkResult();
      }
   }
}