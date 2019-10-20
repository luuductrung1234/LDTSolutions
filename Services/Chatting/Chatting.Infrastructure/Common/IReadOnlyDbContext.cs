using System;
using System.Collections.Generic;
using System.Text;

// SalesHub Common
using SalesHub.Common.GenericMongoDbRepository;

namespace Chatting.Infrastructure.Common
{
   public interface IReadOnlyDbContext : IDbContext
   {
   }
}
