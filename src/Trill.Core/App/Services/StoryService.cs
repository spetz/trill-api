using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Trill.Core.App.Commands;
using Trill.Core.App.Queries;
using Trill.Core.Domain.Entities;
using Trill.Core.Repositories;

namespace Trill.Core.App.Services
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;
        private readonly ILogger<StoryService> _logger;

        public StoryService(IStoryRepository storyRepository, ILogger<StoryService> logger)
        {
            _storyRepository = storyRepository;
            _logger = logger;
        }
    
        public async Task<Story> GetAsync(Guid id)
        {
            return await _storyRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Story>> BrowseAsync(BrowseStories query)
        {
            return await _storyRepository.BrowseAsync(query);
        }

        public async Task AddAsync(SendStory command)
        {
            var story = new Story(command.Id, command.Title, command.Text, command.Author, command.Tags);
            await _storyRepository.AddAsync(story);
            _logger.LogInformation($"Added a story with ID: '{command.Id}'.");
        }
    }
}