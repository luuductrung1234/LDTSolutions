using System;

using LDTSolutions.Common.Domain;

namespace OdeToFood.Core.Models
{
   public class Restaurant : Entity
   {
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

      public void GenerateId()
      {
         Id = Guid.NewGuid();
      }

      public void SetName(string name)
      {
         Name = name;
      }

      public void SetLocation(string location)
      {
         Location = location;
      }

      public void SetCuisineType(CuisineType cuisineType)
      {
         CuisineType = cuisineType;
      }
   }
}
