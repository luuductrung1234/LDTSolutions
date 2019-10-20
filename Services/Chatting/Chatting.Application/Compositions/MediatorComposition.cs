using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

// Chatting Behaviors
using Chatting.Application.Behaviors;

namespace Chatting.Application.Compositions
{
   public static class MediatorComposition
   {
      public static IServiceCollection AddMediator(this IServiceCollection services)
      {
         var assembly = typeof(MediatorComposition).Assembly;
         services.AddMediatR(assembly);
         services.AddAutoMapper(assembly);

         //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
         //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

         return services;
      }
   }
}
