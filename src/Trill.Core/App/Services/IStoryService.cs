using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trill.Core.App.Commands;
using Trill.Core.App.Queries;
using Trill.Core.Domain.Entities;

namespace Trill.Core.App.Services
{
    public interface IStoryService
    {
        Task<Story> GetAsync(Guid id);
        Task<IEnumerable<Story>> BrowseAsync(BrowseStories query);
        Task AddAsync(SendStory command);
    }
}