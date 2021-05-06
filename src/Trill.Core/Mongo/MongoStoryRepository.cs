using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Trill.Core.App.Queries;
using Trill.Core.Domain.Entities;
using Trill.Core.Repositories;

namespace Trill.Core.Mongo
{
    internal class MongoStoryRepository : IStoryRepository
    {
        private readonly IMongoCollection<Story> _collection;

        public MongoStoryRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Story>("stories");
        }
        
        public async Task<Story> GetAsync(Guid id)
        {
            var story = await _collection
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == id);

            return story;
        }

        public async Task<IEnumerable<Story>> BrowseAsync(BrowseStories query)
        {
            var stories = _collection.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.Author))
            {
                stories = stories.Where(x => x.Author == query.Author);
            }

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                stories = stories.Where(x => x.Title.Contains(query.Title));
            }

            return await stories.ToListAsync();
        }

        public Task AddAsync(Story story) => _collection.InsertOneAsync(story);
    }
}