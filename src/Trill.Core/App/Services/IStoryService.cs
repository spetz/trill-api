using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trill.Core.Domain.Entities;

namespace Trill.Core.App.Services
{
    public interface IStoryService
    {
        Task<Story> GetAsync(Guid id);
        Task<IEnumerable<Story>> BrowseAsync();
        Task AddAsync(Story story);
    }
}