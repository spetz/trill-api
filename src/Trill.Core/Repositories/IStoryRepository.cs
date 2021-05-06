using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trill.Core.App.Queries;
using Trill.Core.Domain.Entities;

namespace Trill.Core.Repositories
{
    public interface IStoryRepository
    {
        Task<Story> GetAsync(Guid id);
        Task<IEnumerable<Story>> BrowseAsync(BrowseStories query);
        Task AddAsync(Story story);
    }
}