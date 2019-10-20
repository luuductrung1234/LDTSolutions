using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

// Chatting Domain
using Chatting.Domain;
using Chatting.Domain.Interfaces;

namespace Chatting.Application.Commands.RoomCommands
{
   public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Room>
   {
      private readonly IRoomRepository _roomRepository;
      private readonly IMapper _mapper;
      private readonly ILogger<CreateRoomCommandHandler> _logger;

      public CreateRoomCommandHandler(
         IRoomRepository roomRepository,
         IMapper mapper,
         ILogger<CreateRoomCommandHandler> logger)
      {
         _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
         _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
         _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      }

      public async Task<Room> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
      {
         if(request.UserCodes.Count() < 2)
         {
            throw new ArgumentException("UserCodes parameter must contains more than 2 UserCodes to create Room!");
         }

         Room newRoom = new Room(request.RoomName, request.UserCodes);

         await _roomRepository.AddRoomAsync(newRoom);

         return newRoom;
      }
   }
}
