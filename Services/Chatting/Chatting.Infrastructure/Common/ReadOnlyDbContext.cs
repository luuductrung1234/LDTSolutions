using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

// SalesHub Common
using SalesHub.Common.GenericMongoDbRepository;

namespace Chatting.Infrastructure.Common
{
   public class ReadOnlyDbContext : DbContext, IReadOnlyDbContext
   {
      public ReadOnlyDbContext(IMongoDatabase mongoDatabase)
         : base(mongoDatabase)
      {
      }

      public ReadOnlyDbContext(string connectionString, string databaseName)
         : base(connectionString, databaseName)
      {
      }
   }
}
