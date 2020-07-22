using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using MordorFanficWeb.PresentationAdapters.TagsAdapter;
using MordorFanficWeb.PresentationAdapters.CompositionTagsAdapter;
using MordorFanficWeb.ViewModels.TagsViewModels;
using MordorFanficWeb.ViewModels.CompositionTagsViewModels;

namespace MordorFanficWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : Controller
    {
        private readonly ITagsAdapter tagsAdapter;
        private readonly ICompositionTagsAdapter compositionTagsAdapter;
        private readonly ILogger<TagsController> logger;

        public TagsController(ITagsAdapter tagsAdapter, ICompositionTagsAdapter compositionTagsAdapter, ILogger<TagsController> logger)
        {
            this.tagsAdapter = tagsAdapter;
            this.compositionTagsAdapter = compositionTagsAdapter;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<TagsViewModel>>> GetAllTags()
        {
            try
            {
                var tags = await tagsAdapter.GetAllTags().ConfigureAwait(false);
                if (tags == null)
                {
                    logger.LogError($"Tags list cannot be found.");
                    return NotFound();
                }

                logger.LogInformation("Returned all tags from db");
                return tags;
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetAllTags action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpPost("add-tags")]
        public async Task<ActionResult> AddTags([FromBody] TagsViewModel[] tags)
        {
            try
            {
                if (tags == null)
                {
                    logger.LogError($"Tags objects sent from client is null");
                    return BadRequest("Tags objects is null");
                }

                List<TagsViewModel> listOfTags = new List<TagsViewModel>();
                await Task.Run(() => listOfTags.AddRange(tags)).ConfigureAwait(false);
                await tagsAdapter.CreateRangeOfTags(listOfTags).ConfigureAwait(false);
                logger.LogInformation($"Tags is successfully added");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside AddTags action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpPost]
        public async Task<ActionResult> AddTagsToComposition([FromBody] CompositionTagsViewModel[] tags)
        {
            try
            {
                if (tags == null)
                {
                    logger.LogError($"Tags objects sent from client is null");
                    return BadRequest("Tags objects is null");
                }

                List<CompositionTagsViewModel> listOfTags = new List<CompositionTagsViewModel>();
                await Task.Run(() => listOfTags.AddRange(tags)).ConfigureAwait(false);
                await compositionTagsAdapter.CreateRangeOfTags(listOfTags).ConfigureAwait(false);
                logger.LogInformation($"Tags is successfully added to compositon with id: {tags[0].CompositionId}");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside AddTagsToComposition action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
