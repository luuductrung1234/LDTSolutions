using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;

using LDTSolutions.Common.WebApi.Cors;

namespace LDTSolutions.Common.WebApi.SignalR
{
   public static class ApplicationBuidlerExtensions
   {
      public static IApplicationBuilder UseCustomSignalR<T>(this IApplicationBuilder app, string hubUrl) where T : Hub
      {
         app.UseCustomCors();

         app.UseSignalR((configure) =>
            {
            // signalR server side desired specific transports
            var desiredTransports =
                  HttpTransportType.WebSockets |
                  HttpTransportType.ServerSentEvents;

               configure.MapHub<T>(hubUrl, (options) =>
                  {
                  options.Transports = desiredTransports;

               // the maximum number of bytes from the client that the server buffers
               options.TransportMaxBufferSize = 32;

               // the maximum number of bytes the server can send
               options.ApplicationMaxBufferSize = 32;
               });
            });

         return app;
      }
   }
}
