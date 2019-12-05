using System;

namespace PluralsightIdentityDemo
{
   public class ApplicationUser
   {
      public string Id { get; set; }

      public string UserName { get; set; }

      public string NormalizedUserName { get; set; }

      public string PasswordHash { get; set; }
   }
}
