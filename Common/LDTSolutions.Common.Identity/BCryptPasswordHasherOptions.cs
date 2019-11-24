using BCrypt.Net;
using Microsoft.AspNetCore.Identity;

namespace LDTSolutions.Common.Identity
{
   public class BCryptPasswordHasherOptions : PasswordHasherOptions
   {
      /// <summary>
      /// If
      /// - true : password will be hashed into BCrypt algorithms
      /// - false : use the default implementation algorithms (PBKDF2)
      ///
      /// (*) Notice: if you want to use default implementation of PBKDF2, consider to set CompatibilityMode
      /// </summary>
      public bool EnableBCrypt { get; set; } = true;

      /// <summary>
      /// Highly advisable that you allow the library generate the salt for you.
      ///
      /// BCrypt.Net-Next on hashing will generate a salt automatically for you
      /// with the work factor (2^number of rounds) set to 10
      /// (which matches the default across most implementation and is currently viewed as a good level of security/risk)
      ///
      /// | Cost  | Iterations               |
      /// |-------|--------------------------|
      /// |   8   |    256 iterations        |
      /// |   9   |    512 iterations        |
      /// |  10   |  1,024 iterations        |
      /// |  11   |  2,048 iterations        |
      /// |  12   |  4,096 iterations        |
      /// |  13   |  8,192 iterations        |
      /// |  14   | 16,384 iterations        |
      /// |  15   | 32,768 iterations        |
      /// |  16   | 65,536 iterations        |
      /// |  17   | 131,072 iterations       |
      /// |  18   | 262,144 iterations       |
      /// |  19   | 524,288 iterations       |
      /// |  20   | 1,048,576 iterations     |
      /// |  21   | 2,097,152 iterations     |
      /// |  22   | 4,194,304 iterations     |
      /// |  23   | 8,388,608 iterations     |
      /// |  24   | 16,777,216 iterations    |
      /// |  25   | 33,554,432 iterations    |
      /// |  26   | 67,108,864 iterations    |
      /// |  27   | 134,217,728 iterations   |
      /// |  28   | 268,435,456 iterations   |
      /// |  29   | 536,870,912 iterations   |
      /// |  30   | 1,073,741,824 iterations |
      /// |  31   | 2,147,483,648 iterations |
      /// </summary>
      public int WorkFactor { get; set; } = 11;

      /// <summary>
      /// https://github.com/BcryptNet/bcrypt.net#enhanced-entropy
      /// </summary>
      public bool EnhancedEntropy { get; set; } = false;

      public HashType HashType { get; set; } = HashType.SHA384;
   }
}
