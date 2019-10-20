using MediatR;
using System;
using System.Collections.Generic;

// Chatting Domain
using Chatting.Domain;

namespace Chatting.Application.Commands.ChatMessageCommands
{
   public class CreateChatMessageCommand : IRequest<ChatMessage>
   {
      public string SenderCode { get; set; }

      public IEnumerable<string> ReceiverCodes { get; set; }

      public string RoomCode { get; set; }

      public string Message { get; set; }
   }
}
