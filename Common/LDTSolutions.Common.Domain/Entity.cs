using System;

namespace LDTSolutions.Common.Domain
{
   public abstract class Entity
   {
      public Guid Id { get; protected set; }

      public DateTime CreatedDate { get; protected set; }

      public byte[] Timestamp { get; protected set; }

      public bool IsDeleted { get; protected set; }

      public Entity()
      {
         CreatedDate = DateTime.UtcNow;
         IsDeleted = false;
      }

      public void SetDeleted()
      {
         IsDeleted = true;
      }
   }
}
