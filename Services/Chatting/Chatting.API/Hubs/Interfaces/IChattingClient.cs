using System.Threading.Tasks;

// Chatting Domain
using Chatting.Domain;

namespace Chatting.API.Hubs.Interfaces
{
   public interface IChattingClient
   {
      Task ReceiveSomethingsHappen(string message);

      Task ReceiveMessage(ChatMessage message);

      Task Finished();
   }
}
