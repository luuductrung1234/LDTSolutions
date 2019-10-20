using System;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

// Chatting Domain
using Chatting.Domain.Interfaces;

// Chatting Infrastructure
using Chatting.Infrastructure.DatabaseMapping;
using Chatting.Infrastructure.Repositories;
using Chatting.Infrastructure.Common;

namespace Chatting.Infrastructure.Compositions
{
   public static class InfrastructureComposition
   {
      public static IServiceCollection AddInfrastructure(this IServiceCollection services, IHostingEnvironment env)
      {
         MappingConfigurations.Map();

         services
            .AddMasterDbContext()
            .AddServiceDbContext(env)
            .AddRepositories();

         return services;
      }

      public static IServiceCollection AddMasterDbContext(this IServiceCollection services)
      {
         return services;
      }

      public static IServiceCollection AddServiceDbContext(this IServiceCollection services, IHostingEnvironment env)
      {
         services.AddSingleton<IGenericDbContext>(sp =>
         {
            var dbSettings = sp.GetRequiredService<IOptions<DatabaseSetting>>();

            MongoClientSettings settings = MongoClientSettings.FromUrl(
               new MongoUrl(dbSettings.Value.ConnectionString)
            );
            settings.SslSettings = new SslSettings()
            {
               EnabledSslProtocols = SslProtocols.Tls12
            };

            settings.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;

            if (env.EnvironmentName.ToLower() == "local")
            {
               settings.GuidRepresentation = MongoDB.Bson.GuidRepresentation.CSharpLegacy;
            }

            var mongoClient = new MongoClient(settings);
            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
            return new GenericDbContext(mongoDatabase);
         });

         services.AddSingleton<IReadOnlyDbContext>(sp =>
         {
            var dbSettings = sp.GetRequiredService<IOptions<ReadOnlyDatabaseSetting>>();

            MongoClientSettings settings = MongoClientSettings.FromUrl(
               new MongoUrl(dbSettings.Value.ConnectionString)
            );
            settings.SslSettings = new SslSettings()
            {
               EnabledSslProtocols = SslProtocols.Tls12
            };

            settings.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;

            if (env.EnvironmentName.ToLower() == "local")
            {
               settings.GuidRepresentation = MongoDB.Bson.GuidRepresentation.CSharpLegacy;
            }

            var mongoClient = new MongoClient(settings);
            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
            return new ReadOnlyDbContext(mongoDatabase);
         });

         return services;
      }

      public static IServiceCollection AddRepositories(this IServiceCollection services)
      {
         services.AddScoped<IUserRepository, UserRepository>();
         services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
         services.AddScoped<IRoomRepository, RoomRepository>();

         return services;
      }
   }
}
