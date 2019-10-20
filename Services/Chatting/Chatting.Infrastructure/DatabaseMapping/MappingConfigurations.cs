using MongoDB.Bson.Serialization;

// SalesHub Common
using SalesHub.Common.Core;
using SalesHub.Common.GenericMongoDbRepository.Utilities;

// Chatting Domain
using Chatting.Domain;

namespace Chatting.Infrastructure.DatabaseMapping
{
   public class MappingConfigurations
   {
      public static void Map()
      {
         BsonMappingHelpers.RegisterLocalDateTimeBsonSerializers();
         BsonMappingHelpers.RegisterCamelCaseConvention();

         if (BsonMappingHelpers.IsNeedToMap(typeof(Enumeration)))
         {
            BsonClassMap.RegisterClassMap<Enumeration>(cm =>
            {
               cm.SetIsRootClass(true);
               cm.MapMember(m => m.Id);
               cm.MapMember(m => m.Name);
            });
         }

         if (BsonMappingHelpers.IsNeedToMap(typeof(User)))
         {
            BsonClassMap.RegisterClassMap<User>(map =>
            {
               map.AutoMap();
            });
         }

         if (BsonMappingHelpers.IsNeedToMap(typeof(Room)))
         {
            BsonClassMap.RegisterClassMap<Room>(map =>
            {
               map.AutoMap();
            });
         }

         if (BsonMappingHelpers.IsNeedToMap(typeof(ChatMessage)))
         {
            BsonClassMap.RegisterClassMap<ChatMessage>(map =>
            {
               map.AutoMap();
            });
         }
      }
   }
}
