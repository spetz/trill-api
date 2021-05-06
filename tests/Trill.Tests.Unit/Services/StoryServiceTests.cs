using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Trill.Core.App.Services;
using Trill.Core.Domain.Entities;
using Trill.Core.Repositories;
using Xunit;

namespace Trill.Tests.Unit.Services
{
    public class StoryServiceTests
    {
        [Fact]
        public async Task get_story_given_valid_id_should_return_story()
        {
            // Arrange
            var storyId = Guid.NewGuid();
            _storyRepository.GetAsync(storyId).Returns(new Story(storyId, "test", "test", "test", new List<string>()));
            
            // Act
            var story = await _storyService.GetAsync(storyId);
            
            // Assert
            story.ShouldNotBeNull();
            await _storyRepository.Received(1).GetAsync(storyId);
        }
    
        private readonly IStoryRepository _storyRepository;
        private readonly ILogger<StoryService> _logger;
        private readonly IStoryService _storyService;

        public StoryServiceTests()
        {
            _storyRepository = Substitute.For<IStoryRepository>();
            _logger = Substitute.For<ILogger<StoryService>>();
            _storyService = new StoryService(_storyRepository, _logger);
        }
    }
}