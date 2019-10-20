using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chatting.Domain.Interfaces
{
   public interface IChatMessageRepository
   {
      Task AddChatMessageAsync(ChatMessage chatMessage);

      Task<IEnumerable<ChatMessage>> GetChatMessagesAsync();

      Task<IEnumerable<ChatMessage>> GetChatMessagesByUserAsync(string userCode, string shardKey = null);

      Task<IEnumerable<ChatMessage>> GetChatMessagesByRoomAsync(string roomCode);

      Task<ChatMessage> GetChatMessageAsync(Guid id);

      Task<ChatMessage> GetChatMessageAsync(string messageCode);
   }
}
