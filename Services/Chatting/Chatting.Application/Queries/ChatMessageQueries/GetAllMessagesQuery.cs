using System;
using System.Collections.Generic;
using MediatR;

// Chatting Application Queries
using Chatting.Application.Queries.Representations;

namespace Chatting.Application.Queries.ChatMessageQueries
{
   public class GetAllMessagesQuery : IRequest<IEnumerable<ChatMessageRepresentation>>
   {
   }
}
