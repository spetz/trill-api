using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Trill.Core.App.Commands;
using Trill.Core.App.Services;
using Trill.Core.Domain.Entities;
using Trill.Core.Exceptions;
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
        
        [Fact]
        public async Task add_should_succeed_given_valid_data()
        {
            // Arrange
            var command = new SendStory(Guid.NewGuid(), "test", "Lorem ipsum", "user1", new[] {"tag1", "tag2"});
            
            // Act
            await _storyService.AddAsync(command);
            
            // Assert
            await _storyRepository.Received(1).AddAsync(Arg.Is<Story>(x => x.Id == command.Id));
        }
        
        [Fact]
        public async Task add_should_fail_given_missing_author()
        {
            var command = new SendStory(Guid.NewGuid(), "Test", "Lorem ipsum", string.Empty, new[] {"tag1", "tag2"});
            var exception = await Record.ExceptionAsync(async () => await _storyService.AddAsync(command));
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<MissingAuthorException>();
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