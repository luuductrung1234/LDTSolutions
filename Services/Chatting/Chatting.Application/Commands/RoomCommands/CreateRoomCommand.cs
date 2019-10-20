using MediatR;
using System.Collections.Generic;

// Chatting Domain
using Chatting.Domain;

namespace Chatting.Application.Commands.RoomCommands
{
   public class CreateRoomCommand : IRequest<Room>
   {
      public string RoomName { get; set; }

      public IEnumerable<string> UserCodes { get; set; }
   }
}
