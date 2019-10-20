namespace LDTSolutions.Common.WebApi.Tests.NodeModulesTest
{
    using System;
    using Microsoft.AspNetCore.Builder;

    using LDTSolutions.Common.WebApi.NodeModules;

    public class RequestPathStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseNodeModules(TimeSpan.FromSeconds(600), "/vendor");
        }
    }
}
