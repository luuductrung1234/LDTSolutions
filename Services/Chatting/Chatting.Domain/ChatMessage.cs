using System;
using System.Collections.Generic;

// SalesHub Common
using SalesHub.Common.DDDCore;

namespace Chatting.Domain
{
   public class ChatMessage : Entity, IAggregateRoot
   {
      public string ChatMessageCode { get; private set; }

      public string RoomCode { get; private set; }

      public string SenderCode { get; private set; }

      public IEnumerable<string> ReceiverCodes { get; private set; }

      public DateTime CreatedDate { get; private set; }

      public string Message { get; private set; }

      public ChatMessage(
         string roomCode,
         string senderCode,
         IEnumerable<string> receiverCodes,
         string message)
      {
         ChatMessageCode = GenerateCode();
         RoomCode = roomCode;
         SenderCode = senderCode;
         ReceiverCodes = receiverCodes;

         Message = message;

         CreatedDate = DateTime.Now;
      }

      private string GenerateCode()
      {
         var value = DateTime.Now;
         return $"MSG-{value.ToString("yyyyMMddHHmmssffff")}";
      }
   }
}
