using System;
using System.Collections.Generic;
using System.Text;

namespace OdeToFood.Core.Exceptions
{
   public class RestaurantNotFoundException : DomainException
   {
      public Guid RestaurantId { get; }

      public RestaurantNotFoundException(Guid restaurantId)
         : base($"Restaurant with id:{restaurantId} is not found!")
      {
         RestaurantId = restaurantId;
      }
   }
}
