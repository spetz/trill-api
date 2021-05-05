using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            });
        }
    }
}
