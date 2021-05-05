﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Trill.Core.Domain.Entities;

namespace Trill.Core.App.Services
{
    public class StoryService : IStoryService
    {
        private readonly List<Story> _stories = new()
        {
            new Story(Guid.NewGuid(), "Story 1", "Lorem ipsum 1", "user1", new[] {"tag1", "tag2"}),
            new Story(Guid.NewGuid(), "Story 2", "Lorem ipsum 2", "user1", new[] {"tag2", "tag3"}),
            new Story(Guid.NewGuid(), "Story 3", "Lorem ipsum 3", "user2", new[] {"tag1", "tag3"})
        };
        
        private readonly ILogger<StoryService> _logger;

        public StoryService(ILogger<StoryService> logger)
        {
            _logger = logger;
        }
    
        public async Task<Story> GetAsync(Guid id)
        {
            await Task.CompletedTask;
            return _stories.SingleOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<Story>> BrowseAsync()
        {
            await Task.CompletedTask;
            return _stories;
        }

        public async Task AddAsync(Story story)
        {
            await Task.CompletedTask;
            _stories.Add(story);
            _logger.LogInformation($"Added a story with ID: '{story.Id}'.");
        }
    }
}