using System;

namespace Chatting.Application.Exceptions
{
   public class ChattingRequestValidationException : Exception
   {
      public ChattingRequestValidationException(string requestTypeName, Exception innerException)
         : base($"Fail to handle request: {requestTypeName}", innerException)
      {

      }
   }
}
