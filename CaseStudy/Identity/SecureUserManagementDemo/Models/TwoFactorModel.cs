using System.ComponentModel.DataAnnotations;

namespace SecureUserManagementDemo.Models
{
   public class TwoFactorModel
   {
      [Required]
      public string Token { get; set; }
   }
}
