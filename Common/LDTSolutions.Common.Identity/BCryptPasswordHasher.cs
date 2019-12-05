using System;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LDTSolutions.Common.Identity
{
   /// <summary>
   ///
   /// This class is a derived class of <see cref="Microsoft.AspNetCore.Identity.PasswordHasher{TUser}"/>, the default implementation of <see cref="Microsoft.AspNetCore.Identity.IPasswordHasher{TUser}"/>
   ///
   /// It offer an alternative password hashing algorithms, BCrypt, using BCrypt.Net-Next library
   ///
   /// The default algorithms, PBKDF2, with version 2 & 3 are still remain and available in this implementation.
   ///
   /// Want to implement another custom PasswordHasher? See also:
   /// - https://andrewlock.net/migrating-passwords-in-asp-net-core-identity-with-a-custom-passwordhasher/
   /// - https://www.scottbrady91.com/ASPNET-Identity/Improving-the-ASPNET-Core-Identity-Password-Hasher
   ///
   /// </summary>
   /// <typeparam name="TUser"></typeparam>
   public class BCryptPasswordHasher<TUser> : PasswordHasher<TUser> where TUser : class
   {
      private readonly BCryptPasswordHasherOptions _options;

      public BCryptPasswordHasher(IOptions<BCryptPasswordHasherOptions> options)
         : base(options)
      {
         this._options = options?.Value ?? new BCryptPasswordHasherOptions();
      }

      public override string HashPassword(TUser user, string password)
      {
         if (password == null)
         {
            throw new ArgumentNullException(nameof(password));
         }

         if (_options.EnableBCrypt)
         {
            return HashPasswordBCrypt(password);
         }
         else
         {
            return base.HashPassword(user, password);
         }
      }

      private string HashPasswordBCrypt(string password)
      {
         string passwordHash = string.Empty;
         if (_options.EnhancedEntropy)
         {
            passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, _options.HashType, _options.WorkFactor);
         }
         else
         {
            passwordHash = BCrypt.Net.BCrypt.HashPassword(password, _options.WorkFactor);
         }

         string formatedPasswordHash = ConvertPasswordFormat(passwordHash, 0xFF);

         return formatedPasswordHash;
      }

      private static string ConvertPasswordFormat(string passwordHash, byte formatMarker)
      {
         var bytes = Encoding.UTF8.GetBytes(passwordHash);
         var bytesWithMarker = new byte[bytes.Length + 1];
         bytesWithMarker[0] = formatMarker;
         bytes.CopyTo(bytesWithMarker, 1);
         return Convert.ToBase64String(bytesWithMarker);
      }

      public override PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
      {
         if (hashedPassword == null)
         {
            throw new ArgumentNullException(nameof(hashedPassword));
         }

         if (providedPassword == null)
         {
            throw new ArgumentNullException(nameof(providedPassword));
         }


         byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

         // read the format marker from the hashed password
         if (decodedHashedPassword.Length == 0)
         {
            return PasswordVerificationResult.Failed;
         }

         // ASP.NET Core uses 0x00 and 0x01, so we start at the other end
         if (decodedHashedPassword[0] == 0xFF)
         {
            return VerifyHashedPasswordBCrypt(decodedHashedPassword, providedPassword)
               ? PasswordVerificationResult.Success
               : PasswordVerificationResult.Failed;
         }
         else
         {
            if (base.VerifyHashedPassword(user, hashedPassword, providedPassword) != PasswordVerificationResult.Failed)
            {
               // This is an old password hash format - the caller needs to rehash if we're not running in an older compat mode.
               return _options.EnableBCrypt
                  ? PasswordVerificationResult.SuccessRehashNeeded
                  : PasswordVerificationResult.Success;
            }
            else
            {
               return PasswordVerificationResult.Failed;
            }
         }
      }

      private bool VerifyHashedPasswordBCrypt(byte[] hashedPassword, string providedPassword)
      {
         if (hashedPassword.Length < 2)
         {
            return false; // bad size
         }

         //convert back to string for BCrypt, ignoring first byte
         var storedHash = Encoding.UTF8.GetString(hashedPassword, 1, hashedPassword.Length - 1);

         if (_options.EnhancedEntropy)
         {
            return BCrypt.Net.BCrypt.Verify(providedPassword, storedHash);
         }
         else
         {
            return BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, storedHash, _options.HashType);
         }
      }
   }
}
