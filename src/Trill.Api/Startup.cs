using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Trill.Core.Domain.Entities;

namespace Trill.Api
{
    public class Startup
    {
        private readonly List<Story> _stories = new()
        {
            new Story(Guid.NewGuid(), "Story 1", "Lorem ipsum 1", "user1", new[] {"tag1", "tag2"}),
            new Story(Guid.NewGuid(), "Story 2", "Lorem ipsum 2", "user1", new[] {"tag2", "tag3"}),
            new Story(Guid.NewGuid(), "Story 3", "Lorem ipsum 3", "user2", new[] {"tag1", "tag3"}),
        };
        
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });

                endpoints.MapGet("stories", async context =>
                {
                    await context.Response.WriteAsJsonAsync(_stories);
                });
                
                endpoints.MapGet("stories/{storyId:guid}", async context =>
                {
                    var storyId = Guid.Parse(context.Request.RouteValues["storyId"].ToString());
                    var story = _stories.SingleOrDefault(x => x.Id == storyId);
                    if (story is null)
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        return;
                    }

                    await context.Response.WriteAsJsonAsync(story);
                });
                
                endpoints.MapPost("stories", async context =>
                {
                    var story = await context.Request.ReadFromJsonAsync<Story>();
                    if (story is null)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return;
                    }

                    _stories.Add(story);
                    context.Response.Headers.Add(HttpResponseHeader.Location.ToString(), $"stories/{story.Id}");
                    context.Response.StatusCode = StatusCodes.Status201Created;
                });
            });
        }
    }
}
