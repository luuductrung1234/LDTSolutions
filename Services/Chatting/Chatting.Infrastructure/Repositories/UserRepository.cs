using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// Common
using SalesHub.Common.GenericMongoDbRepository;

// Chatting Domain
using Chatting.Domain;
using Chatting.Domain.Interfaces;

// Chatting Infrastructure
using Chatting.Infrastructure.Common;
using SalesHub.Common.Core.LINQ;

namespace Chatting.Infrastructure.Repositories
{
   public class UserRepository : BaseRepository<User>, IUserRepository
   {
      public UserRepository(IGenericDbContext dbContext)
         : base(dbContext, partitionKey: "userCode")
      {

      }
      public async Task AddUserAsync(User user)
      {
         await this.AddOneAsync(user);
      }

      public async Task<User> GetUserAsync(Guid id)
      {
         var filter = PredicateBuilder.Create<User>(user => user.Id == id
            && user.IsDeleted == false);

         return await GetOneAsync(filter);
      }

      public async Task<User> GetUserAsync(string userCode)
      {
         var filter = PredicateBuilder.Create<User>(user => user.UserCode == userCode
            && user.IsDeleted == false);

         return await GetOneAsync(filter);
      }

      public async Task<IEnumerable<User>> GetUsersAsync()
      {
         var filter = PredicateBuilder.Create<User>(user => user.IsDeleted == false);

         return await GetAllAsync(filter);
      }
   }
}
