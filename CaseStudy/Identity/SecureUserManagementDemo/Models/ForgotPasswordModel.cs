using System.ComponentModel.DataAnnotations;

namespace SecureUserManagementDemo.Models
{
   public class ForgotPasswordModel
   {
      [Required]
      [EmailAddress]
      public string Email { get; set; }
   }
}
