using Microsoft.Extensions.DependencyInjection;

using LDTSolutions.Common.WebApi.Cors;

namespace LDTSolutions.Common.WebApi.SignalR
{
   public static class ServiceCollectionExtensions
   {
      public static IServiceCollection AddCustomSignalR(this IServiceCollection services)
      {
         services
            .AddCustomCors()
            .AddHttpContextAccessor()
            .AddSignalR(c => c.EnableDetailedErrors = true);
         //.AddJsonProtocol(options =>
         //{
         //   // customize how Json format data
         //   options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver();
         //})
         //.AddMessagePackProtocol(options =>
         //{
         //   // customize how MessagePack format data
         //   options.FormatterResolvers = new List<MessagePack.IFormatterResolver>()
         //   {
         //      MessagePack.Resolvers.StandardResolver.Instance
         //   };
         //});

         return services;
      }
   }
}
