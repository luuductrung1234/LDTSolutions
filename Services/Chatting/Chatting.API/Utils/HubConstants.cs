using Chatting.Common.Extensions;
using Chatting.Domain;

namespace Chatting.API.Utils
{
   public static class HubConstants
   {
      public static readonly string UserIdentityKey = nameof(User.UserCode).ToCamelCase();
   }
}
