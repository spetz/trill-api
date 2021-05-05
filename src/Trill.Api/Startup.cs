using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Trill.Core;
using Trill.Core.App.Services;
using Trill.Core.Domain.Entities;

namespace Trill.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiOptions>(_configuration.GetSection("api"));
            
            services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddSingleton<ErrorHandlerMiddleware>();

            services.AddCore();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.Use(async (ctx, next) =>
            {
                if (ctx.Request.Query.TryGetValue("token", out var token) && token == "secret")
                {
                    await ctx.Response.WriteAsync("Secret");
                    return;
                }

                await next();
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var apiOptions = context.RequestServices.GetRequiredService<IOptions<ApiOptions>>();
                    await context.Response.WriteAsync(apiOptions.Value.Name);
                });

                endpoints.MapGet("stories", async context =>
                {
                    var storyService = context.RequestServices.GetRequiredService<IStoryService>();
                    var stories = await storyService.BrowseAsync();
                    await context.Response.WriteAsJsonAsync(stories);
                });
                
                endpoints.MapGet("stories/{storyId:guid}", async context =>
                {
                    var storyId = Guid.Parse(context.Request.RouteValues["storyId"].ToString());
                    var storyService = context.RequestServices.GetRequiredService<IStoryService>();
                    var story = await storyService.GetAsync(storyId);
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

                    var storyService = context.RequestServices.GetRequiredService<IStoryService>();
                    await storyService.AddAsync(story);
                    context.Response.Headers.Add(HttpResponseHeader.Location.ToString(), $"stories/{story.Id}");
                    context.Response.StatusCode = StatusCodes.Status201Created;
                });
            });
        }
    }
}
