using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecureUserManagementDemo.Models
{
   public class ProfilesModel
   {
      public string UserName { get; set; }

      public string Email { get; set; }

      [DataType(DataType.Password)]
      public string Password { get; set; }

      [DataType(DataType.Password)]
      public string NewPassword { get; set; }

      [DataType(DataType.Password)]
      [Compare("NewPassword")]
      public string ConfirmPassword { get; set; }
   }
}
