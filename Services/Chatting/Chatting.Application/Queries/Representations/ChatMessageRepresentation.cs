using System;
using System.Collections.Generic;

namespace Chatting.Application.Queries.Representations
{
   public class ChatMessageRepresentation
   {
      public Guid Id { get; set; }

      public string ChatMessageCode { get; set; }

      public string RoomCode { get; set; }

      public string SenderCode { get; set; }

      public IEnumerable<string> ReceiverCodes { get; set; }

      public DateTime CreatedDate { get; set; }

      public string Message { get; set; }
   }
}
