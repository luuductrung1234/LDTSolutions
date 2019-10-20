using System;

namespace Chatting.Domain.Exceptions
{
   public class ChattingDomainException : Exception
   {
      public string ErrorCode { get; set; }

      public ChattingDomainException(string errorCode, string message)
         : base($"ErrorCode:{errorCode}.\n {message}")
      {
         ErrorCode = errorCode;
      }

      public ChattingDomainException(string errorCode, string message, Exception innerException)
         : base($"ErrorCode:{errorCode}.\n {message}", innerException)
      {
         ErrorCode = errorCode;
      }
   }
}
