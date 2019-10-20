using System;

namespace Chatting.Common.Extensions
{
   public static class StringExtensions
   {
      public static string ToCamelCase(this string value)
      {
         value = value.Replace("_", string.Empty);
         if (!string.IsNullOrEmpty(value) && value.Length > 1)
         {
            value = Char.ToLowerInvariant(value[0]) + value.Substring(1);
         }

         return value;
      }
   }
}
