using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecureUserManagementDemo.Models
{
   public class RegisterModel
   {
      [DataType(DataType.EmailAddress)]
      public string Email { get; set; }

      public string UserName { get; set; }

      [DataType(DataType.Password)]
      public string Password { get; set; }

      [Compare("Password")]
      [DataType(DataType.Password)]
      public string ConfirmPassword { get; set; }
   }
}
