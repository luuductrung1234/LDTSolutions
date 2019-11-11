using System;
using System.Collections.Generic;
using System.Text;

namespace OdeToFood.Core.Exceptions
{
   public class RestaurantIsAlreadyDeleted : DomainException
   {
      public RestaurantIsAlreadyDeleted(Guid restaurantId)
         : base($"Restaurant with id:{restaurantId} is already deleted")
      {
         RestaurantId = restaurantId;
      }

      public Guid RestaurantId { get; }
   }
}
