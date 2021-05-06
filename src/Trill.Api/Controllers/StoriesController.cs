using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Trill.Core.App.Commands;
using Trill.Core.App.Queries;
using Trill.Core.App.Services;
using Trill.Core.Domain.Entities;

namespace Trill.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IStoryService _storyService;

        public StoriesController(IStoryService storyService)
        {
            _storyService = storyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Story>>> Get([FromQuery] BrowseStories query)
            => Ok(await _storyService.BrowseAsync(query));

        [HttpGet("{storyId:guid}")]
        public async Task<ActionResult<Story>> Get(Guid storyId)
        {
            var story = await _storyService.GetAsync(storyId);
            if (story is null)
            {
                return NotFound();
            }

            return Ok(story);
        }

        [HttpPost]
        public async Task<ActionResult> Post(SendStory story)
        {
            await _storyService.AddAsync(story);
            return CreatedAtAction(nameof(Get), new {storyId = story.Id}, null);
        }
    }
}