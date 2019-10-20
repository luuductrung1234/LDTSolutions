using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

// Chatting Domain
using Chatting.Domain;
using Chatting.Domain.Interfaces;
using Chatting.Domain.Exceptions;

namespace Chatting.Application.Commands.ChatMessageCommands
{
   public class CreateChatMessageCommandHandler : IRequestHandler<CreateChatMessageCommand, ChatMessage>
   {
      private readonly IRoomRepository _roomRepository;
      private readonly IChatMessageRepository _chatMessageRepository;
      private readonly IMapper _mapper;
      private readonly ILogger<CreateChatMessageCommandHandler> _logger;

      public CreateChatMessageCommandHandler(
         IRoomRepository roomRepository,
         IChatMessageRepository chatMessageRepository,
         IMapper mapper,
         ILogger<CreateChatMessageCommandHandler> logger)
      {
         _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
         _chatMessageRepository = chatMessageRepository ?? throw new ArgumentNullException(nameof(chatMessageRepository));
         _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
         _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      }

      public async Task<ChatMessage> Handle(CreateChatMessageCommand request, CancellationToken cancellationToken)
      {
         var roomUserCodes = new List<string>();
         roomUserCodes.Add(request.SenderCode);
         roomUserCodes.AddRange(request.ReceiverCodes);
         if (roomUserCodes.Count < 2)
         {
            throw new ArgumentException("UserCodes parameter must contains more than 2 UserCodes to create ChatMessage!");
         }

         bool isRoomExisted = await _roomRepository.CheckRoomExistedAsync(request.RoomCode, roomUserCodes);
         if (!isRoomExisted)
         {
            throw new RoomNotFoundException(request.RoomCode, roomUserCodes);
         }

         var newMessage = new ChatMessage(
            request.RoomCode,
            request.SenderCode,
            request.ReceiverCodes,
            request.Message);

         await _chatMessageRepository.AddChatMessageAsync(newMessage);

         return newMessage;
      }
   }
}
