using Microsoft.Extensions.DependencyInjection;
using Trill.Core.App.Services;

namespace Trill.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IStoryService, StoryService>();

            return services;
        }
    }
}