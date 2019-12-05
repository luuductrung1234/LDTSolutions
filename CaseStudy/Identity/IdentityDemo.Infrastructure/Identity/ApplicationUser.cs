using System;
using Microsoft.AspNetCore.Identity;

namespace IdentityDemo.Infrastructure.Identity
{
   public class ApplicationUser : IdentityUser<Guid>
   {
      public string Locale { get; set; } = "en-GB";

      public string OrgId { get; set; }
   }

   public class Organization
   {
      public string Id { get; set; }

      public string Name { get; set; }
   }
}
