using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chatting.Domain.Interfaces
{
   public interface IUserRepository
   {
      Task AddUserAsync(User user);

      Task<IEnumerable<User>> GetUsersAsync();

      Task<User> GetUserAsync(Guid id);

      Task<User> GetUserAsync(string userCode);
   }
}
