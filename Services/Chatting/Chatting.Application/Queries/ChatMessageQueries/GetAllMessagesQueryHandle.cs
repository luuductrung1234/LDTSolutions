using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

// Chatting Domain
using Chatting.Domain;
using Chatting.Domain.Interfaces;

// Chatting Application Queries
using Chatting.Application.Queries.Representations;

namespace Chatting.Application.Queries.ChatMessageQueries
{
   public class GetAllMessagesQueryHandle : IRequestHandler<GetAllMessagesQuery, IEnumerable<ChatMessageRepresentation>>
   {
      private readonly IChatMessageRepository _chatMessageRepository;
      private readonly IMapper _mapper;

      public GetAllMessagesQueryHandle(
         IChatMessageRepository chatMessageRepository,
         IMapper mapper)
      {
         _chatMessageRepository = chatMessageRepository ?? throw new ArgumentNullException(nameof(chatMessageRepository));
         _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      }

      public async Task<IEnumerable<ChatMessageRepresentation>> Handle(GetAllMessagesQuery request, CancellationToken cancellationToken)
      {
         IEnumerable<ChatMessage> messages = await _chatMessageRepository.GetChatMessagesAsync();

         var response = _mapper.Map<IEnumerable<ChatMessageRepresentation>>(messages);

         return response;
      }
   }
}
