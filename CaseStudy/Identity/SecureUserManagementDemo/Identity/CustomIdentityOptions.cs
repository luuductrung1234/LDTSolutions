using System;
using Microsoft.AspNetCore.Identity;

namespace SecureUserManagementDemo.Identity
{
   public class CustomIdentityOptions
   {
      /// <summary>
      /// Gets or sets the token provider used to generate tokens used in account confirmation emails.
      /// </summary>
      /// <value>
      /// The <see cref="IUserTwoFactorTokenProvider{TUser}"/> used to generate tokens used in account confirmation emails.
      /// </value>
      public static readonly string EmailConfirmationTokenProvider = "EmailConfirm";
   }
}
