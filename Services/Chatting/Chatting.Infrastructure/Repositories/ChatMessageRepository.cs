using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

// Common
using SalesHub.Common.GenericMongoDbRepository;
using SalesHub.Common.Core.LINQ;

// Chatting Domain
using Chatting.Domain;
using Chatting.Domain.Interfaces;

// Chatting Infrastructure
using Chatting.Infrastructure.Common;

namespace Chatting.Infrastructure.Repositories
{
   public class ChatMessageRepository : BaseRepository<ChatMessage>, IChatMessageRepository
   {
      public ChatMessageRepository(IGenericDbContext dbContext)
         : base(dbContext, partitionKey: "roomCode")
      {

      }

      public async Task AddChatMessageAsync(ChatMessage chatMessage)
      {
         await this.AddOneAsync(chatMessage);
      }

      public async Task<ChatMessage> GetChatMessageAsync(Guid id)
      {
         var filter = PredicateBuilder.Create<ChatMessage>(m => m.Id == id
            && m.IsDeleted == false);

         return await this.GetOneAsync(filter);
      }

      public async Task<ChatMessage> GetChatMessageAsync(string messageCode)
      {
         var filter = PredicateBuilder.Create<ChatMessage>(m => m.ChatMessageCode == messageCode
            && m.IsDeleted == false);

         return await this.GetOneAsync(filter);
      }

      public async Task<IEnumerable<ChatMessage>> GetChatMessagesAsync()
      {
         var filter = PredicateBuilder.Create<ChatMessage>(m => m.IsDeleted == false);

         return await this.GetAllAsync(filter);
      }

      public async Task<IEnumerable<ChatMessage>> GetChatMessagesByRoomAsync(string roomCode)
      {
         var filter = PredicateBuilder.Create<ChatMessage>(m => m.RoomCode == roomCode
            && m.IsDeleted == false);

         return await this.GetAllAsync(filter);
      }

      public async Task<IEnumerable<ChatMessage>> GetChatMessagesByUserAsync(string userCode, string shardKey = null)
      {
         var filter = PredicateBuilder.Create<ChatMessage>(m => m.IsDeleted == false);
         filter = filter.And(m => m.SenderCode == userCode || m.ReceiverCodes.Contains(userCode));

         return await this.GetAllAsync(filter);
      }
   }
}
