using System;
using System.Collections.Generic;
using System.Linq;
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
            await Task.CompletedTask;
            return _stories.SingleOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<Story>> BrowseAsync(BrowseStories query)
        {
            await Task.CompletedTask;
            return stories;
        }

        public async Task AddAsync(SendStory command)
        {
            await Task.CompletedTask;
            var story = new Story(command.Id, command.Title, command.Text, command.Author, command.Tags);
            _stories.Add(story);
            _logger.LogInformation($"Added a story with ID: '{command.Id}'.");
        }
    }
}