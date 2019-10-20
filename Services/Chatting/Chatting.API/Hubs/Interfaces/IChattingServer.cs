using System.Threading.Tasks;

// Chatting Application Commands
using Chatting.Application.Commands.ChatMessageCommands;

namespace Chatting.API.Hubs.Interfaces
{
   public interface IChattingServer
   {
      Task DoSomethings(int seed);

      Task SendMessage(CreateChatMessageCommand request);
   }
}
