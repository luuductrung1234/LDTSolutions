using System;
using System.Collections.Generic;

namespace Chatting.Domain.Exceptions
{
   public class RoomNotFoundException : ChattingDomainException
   {
      public RoomNotFoundException(string roomCode)
         : base(ErrorCodes.RoomNotFound, $"Room with code:{roomCode} is not found!")
      {

      }

      public RoomNotFoundException(string roomCode, IEnumerable<string> userCodes)
         : base(ErrorCodes.RoomNotFound, $"Room with code:{roomCode}\n " +
              $"and with userCodes:{string.Join(",", userCodes)} is not found!")
      {

      }
   }
}
