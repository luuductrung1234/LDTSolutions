using System;

// SalesHub Common
using SalesHub.Common.DDDCore;

namespace Chatting.Domain
{
   public class User : Entity, IAggregateRoot
   {
      public string UserCode { get; private set; }

      public string UserName { get; private set; }

      public string FirstName { get; private set; }

      public string LastName { get; private set; }

      public string Password { get; private set; }

      public DateTime? Birth { get; private set; }

      public User(
         string userName,
         string password,
         string firstName,
         string lastName,
         DateTime? birth)
      {
         UserCode = GenerateCode();
         UserName = userName;
         Password = password;
         FirstName = firstName;
         LastName = lastName;
         Birth = birth;
      }

      private string GenerateCode()
      {
         var value = DateTime.Now;
         return $"USR-{value.ToString("yyyyMMddHHmmssffff")}";
      }
   }
}
