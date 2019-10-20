using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

using SalesHub.Common.Core.DependencyInjection;

namespace Identity.API
{
   /// <summary>
   /// The FabricRuntime creates an instance of this class for each service type instance. 
   /// </summary>
   internal sealed class IdentityService : StatelessService
   {
      public IdentityService(StatelessServiceContext context)
          : base(context)
      { }

      /// <summary>
      /// Optional override to create listeners (like tcp, http) for this service instance.
      /// </summary>
      /// <returns>The collection of listeners.</returns>
      protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
      {
         return new ServiceInstanceListener[]
         {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        return new WebHostBuilder()
                                    .UseKestrel()
                                    .ConfigureServices(
                                        (services) => services
                                             .AddSingleton<StatelessServiceContext>(serviceContext))
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .ConfigureAppConfiguration((builder) => builder.RegisterSFConfig())
                                    .UseStartup<Startup>()
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url)
                                    .Build();
                    }))
         };
      }
   }
}
