namespace LDTSolutions.Common.WebApi.Tests.NodeModulesTest
{
   using System;
   using System.IO;
   using System.Net;

   using Microsoft.AspNetCore.Hosting;
   using Microsoft.AspNetCore.TestHost;

   using Xunit;

   public class UseNodeModulesIntegrationTest
   {
      [Fact]
      public async void RespondsToFileInNodeModules()
      {
         var builder = new WebHostBuilder();
         builder.UseContentRoot(Directory.GetCurrentDirectory());
         builder.UseStartup<TestStartup>();
         var server = new TestServer(builder);
         var client = server.CreateClient();

         var result = await client.GetAsync("/node_modules/hello.txt");

         Assert.Equal(TimeSpan.FromSeconds(600), result.Headers.CacheControl.MaxAge);
         Assert.Equal("hello!", await result.Content.ReadAsStringAsync());
      }

      [Fact]
      public async void IgnoresOutOfNodeModules()
      {
         var builder = new WebHostBuilder();
         builder.UseContentRoot(Directory.GetCurrentDirectory());
         builder.UseStartup<TestStartup>();
         var server = new TestServer(builder);
         var client = server.CreateClient();

         var result = await client.GetAsync("hello.txt");

         Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
      }

      [Fact]
      public async void RespondsToFileInRequestPath()
      {
         var builder = new WebHostBuilder();
         builder.UseContentRoot(Directory.GetCurrentDirectory());
         builder.UseStartup<RequestPathStartup>();
         var server = new TestServer(builder);
         var client = server.CreateClient();

         var result = await client.GetAsync("/vendor/hello.txt");

         Assert.Equal(TimeSpan.FromSeconds(600), result.Headers.CacheControl.MaxAge);
         Assert.Equal("hello!", await result.Content.ReadAsStringAsync());
      }


   }
}
