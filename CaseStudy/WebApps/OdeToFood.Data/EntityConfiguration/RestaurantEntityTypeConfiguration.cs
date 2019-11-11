using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// LDTSolutions Common
using LDTSolutions.Common.EntityFrameworkCore.EntityConfiguration;

using OdeToFood.Core.Models;

namespace OdeToFood.Data.EntityConfiguration
{
   public class RestaurantEntityTypeConfiguration : EntityTypeConfiguration<Restaurant>
   {
      public override void Configure(EntityTypeBuilder<Restaurant> builder)
      {
         builder.Property(r => r.Name).IsRequired();
         builder.Property(r => r.Location).IsRequired();
         builder.Property(r => r.CuisineType).IsRequired();

         base.Configure(builder);
      }
   }
}
