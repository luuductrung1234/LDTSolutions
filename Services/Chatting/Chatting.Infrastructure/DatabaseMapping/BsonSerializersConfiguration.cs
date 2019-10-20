using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace Chatting.Infrastructure.DatabaseMapping
{
   public static class BsonSerializersConfiguration
   {
      public static void RegisterSerializers()
      {
         BsonSerializer.RegisterSerializer(typeof(DateTime), new BsonLocalDateTimeSerializer());
      }
   }

   public class BsonLocalDateTimeSerializer : DateTimeSerializer
   {
      public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
      {
         var dateTime = new DateTime(base.Deserialize(context, args).Ticks, DateTimeKind.Utc);

         dateTime = dateTime.AddHours(7);
         return dateTime;
      }

      public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
      {
         var utcValue = new DateTime(value.Ticks, DateTimeKind.Utc);
         base.Serialize(context, args, utcValue);
      }
   }
}
