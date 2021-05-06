using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Trill.Api;
using Trill.Core.Domain.Entities;
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
            content.ShouldBe("Trill API [test]");
        }
        
        [Fact]
        public async Task get_story_should_return_not_found_given_invalid_id()
        {
            var storyId = Guid.NewGuid();
            var response = await _client.GetAsync($"api/stories/{storyId}");
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
        
        [Fact]
        public async Task get_story_should_return_story_given_valid_id()
        {
            var story = new Story(Guid.NewGuid(), "Test", "Lorem ipsum", "user1", new[] {"tag1", "tag2"});
            await _storyRepository.AddAsync(story);
            var response = await _client.GetAsync($"api/stories/{story.Id}");
            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var storyResponse = await response.Content.ReadFromJsonAsync<Story>();
            storyResponse.ShouldNotBeNull();
            storyResponse.Id.ShouldBe(story.Id);
        }

        private readonly IStoryRepository _storyRepository;
        private readonly HttpClient _client;

        public ControllerTests(WebApplicationFactory<Startup> factory)
        {
            // Arrange
            _storyRepository = new InMemoryStoryRepository();
            _client = factory
                .WithWebHostBuilder(webBuilder =>
                {
                    webBuilder.ConfigureServices(services =>
                    {
                        services.AddSingleton(_storyRepository);
                    });
                    webBuilder.UseEnvironment("test");
                })
                .CreateClient();
        }
    }
}