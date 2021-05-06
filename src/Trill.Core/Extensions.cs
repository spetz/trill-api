using Microsoft.Extensions.DependencyInjection;
using Trill.Core.App.Services;
using Trill.Core.Mongo;
using Trill.Core.Repositories;

namespace Trill.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IStoryService, StoryService>();
            services.AddSingleton<IStoryRepository, InMemoryStoryRepository>();
            services.AddMongo();

            return services;
        }
    }
}