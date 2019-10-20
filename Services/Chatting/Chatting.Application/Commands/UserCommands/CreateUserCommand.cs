using MediatR;
using System;

// Chatting Domain
using Chatting.Domain;

namespace Chatting.Application.Commands.UserCommands
{
   public class CreateUserCommand : IRequest<User>
   {
      public string UserName { get; set; }

      public string Password { get; set; }

      public string FirstName { get; set; }

      public string LastName { get; set; }

      public DateTime? Birth { get; set; }
   }
}
