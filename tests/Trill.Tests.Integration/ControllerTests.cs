using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Trill.Api;
using Trill.Core.Repositories;
using Xunit;

namespace Trill.Tests.Integration
{
    public class ControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        [Fact]
        public async Task get_home_endpoint_should_return_hello_message()
        {
            // Act
            var response = await _client.GetAsync("api");
            
            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.ShouldBe("Trill API [dev]");
        }
        
        private readonly HttpClient _client;

        public ControllerTests(WebApplicationFactory<Startup> factory)
        {
            // Arrange
            _client = factory
                .WithWebHostBuilder(webBuilder =>
                {
                    // webBuilder.ConfigureServices(services =>
                    // {
                    //     services.AddSingleton<IStoryRepository, InMemoryStoryRepository>();
                    // });
                    webBuilder.UseEnvironment("test");
                })
                .CreateClient();
        }
    }
}