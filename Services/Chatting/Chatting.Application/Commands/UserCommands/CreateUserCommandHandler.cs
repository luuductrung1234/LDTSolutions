using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

// Chatting Domain
using Chatting.Domain;
using Chatting.Domain.Interfaces;

namespace Chatting.Application.Commands.UserCommands
{
   public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
   {
      private readonly IUserRepository _userRepository;
      private readonly IMapper _mapper;
      private readonly ILogger<CreateUserCommandHandler> _logger;

      public CreateUserCommandHandler(
         IUserRepository userRepository, 
         IMapper mapper,
         ILogger<CreateUserCommandHandler> logger)
      {
         _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
         _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
         _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      }

      public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
      {
         var newUser = new User(request.UserName, 
            request.Password, 
            request.FirstName,
            request.LastName,
            request.Birth);

         await _userRepository.AddUserAsync(newUser);

         return newUser;
      }
   }
}
