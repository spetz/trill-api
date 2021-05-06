using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trill.Core.App.Queries;
using Trill.Core.Domain.Entities;

namespace Trill.Core.Repositories
{
    public class InMemoryStoryRepository : IStoryRepository
    {
        // Not thread-safe (use e.g. Concurrent collections instead)
        private readonly List<Story> _stories = new();
        
        public async Task<Story> GetAsync(Guid id)
        {
            await Task.CompletedTask;
            return _stories.SingleOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<Story>> BrowseAsync(BrowseStories query)
        {
            await Task.CompletedTask;
            var stories = _stories.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(query.Author))
            {
                stories = stories.Where(x => x.Author == query.Author);
            }

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                stories = stories.Where(x => x.Title.Contains(query.Title));
            }

            return stories;
        }

        public async Task AddAsync(Story story)
        {
            await Task.CompletedTask;
            _stories.Add(story);
        }
    }
}