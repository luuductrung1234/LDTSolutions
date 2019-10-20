using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

// SalesHub Common
using SalesHub.Common.GenericMongoDbRepository;

namespace Chatting.Infrastructure.Common
{
   public class GenericDbContext : DbContext, IGenericDbContext
   {
      public GenericDbContext(IMongoDatabase mongoDatabase)
         : base(mongoDatabase)
      {
      }

      public GenericDbContext(string connectionString, string databaseName)
         : base(connectionString, databaseName)
      {
      }
   }
}
