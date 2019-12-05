using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using LDTSolutions.Common.Domain;

namespace LDTSolutions.Common.EntityFrameworkCore.EntityConfiguration
{
   public abstract class EntityTypeConfiguration<TBase> : IEntityTypeConfiguration<TBase> where TBase : Entity
   {
      public virtual void Configure(EntityTypeBuilder<TBase> builder)
      {
         builder.HasKey(x => x.Id);
         builder.Property(x => x.CreatedDate);
         builder.Property(x => x.Timestamp).IsRowVersion().IsConcurrencyToken();
         builder.Property(x => x.IsDeleted);
      }
   }
}
