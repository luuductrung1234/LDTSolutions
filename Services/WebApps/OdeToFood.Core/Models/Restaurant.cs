﻿using System;

namespace OdeToFood.Core.Models
{
   public class Restaurant
   {
      public Guid Id { get; private set; }

      public string Name { get; private set; }

      public string Location { get; private set; }

      public CuisineType CuisineType { get; private set; }

      public Restaurant(Guid id, string name, string location, CuisineType cuisineType)
      {
         Id = id;
         Name = name;
         Location = location;
         CuisineType = cuisineType;
      }

      public Restaurant(string name, string location, CuisineType cuisineType)
      {
         Name = name;
         Location = location;
         CuisineType = cuisineType;
      }
   }
}
