using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using MordorFanficWeb.PresentationAdapters.CompositionAdapter;
using MordorFanficWeb.ViewModels.CompositionViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using MordorFanficWeb.PresentationAdapters.CompositionRatingsAdapter;
using MordorFanficWeb.PresentationAdapters.CommentsAdapter;
using MordorFanficWeb.ViewModels.CompositionCommentsViewModels;
using MordorFanficWeb.ViewModels.CompositionRatingsViewModels;
using MordorFanficWeb.ViewModels.ChapterViewModels;
using MordorFanficWeb.BusinessLogic.Interfaces;

namespace MordorFanficWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompositionController : Controller
    {
        private readonly ICompositionAdapter compositionAdapter;
        private readonly ICompositionRatingsAdapter ratingsAdapter;
        private readonly ICommentsAdapter commentsAdapter;
        private readonly ILogger<CompositionController> logger;
        private readonly ICloudStorageService storageService;

        public CompositionController(ICompositionAdapter compositionAdapter, ICompositionRatingsAdapter ratingsAdapter, 
            ICommentsAdapter commentsAdapter, ILogger<CompositionController> logger, ICloudStorageService storageService)
        {
            this.compositionAdapter = compositionAdapter;
            this.ratingsAdapter = ratingsAdapter;
            this.commentsAdapter = commentsAdapter;
            this.logger = logger;
            this.storageService = storageService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompositionViewModel>> GetCompositionById(int id)
        {
            try
            {
                var composition = await compositionAdapter.GetComposition(id).ConfigureAwait(false);

                if(composition == null)
                {
                    logger.LogError($"Composition with id: {id}, hasn't been found in db.");
                    return NotFound(id);
                }

                logger.LogInformation($"Returned composition with id: {id}");
                return composition;
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetCompositionById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<CompositionViewModel>>> GetAllCompositions()
        {
            try
            {
                var compositions = await compositionAdapter.GetAllCompositions().ConfigureAwait(false);
                if(compositions == null)
                {
                    logger.LogError($"Compositions list cannot be found.");
                    return NotFound();
                }

                logger.LogInformation("Returned all compositions from db");
                return compositions;
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetAllCompositions action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get-account-compositions/{id}")]
        public async Task<ActionResult<List<CompositionViewModel>>> GetAllCompositionsOfAccount(int id)
        {
            try
            {
                var compositions = await compositionAdapter.GetAllCompositionsOfAccount(id).ConfigureAwait(false);

                if (compositions == null)
                {
                    logger.LogError($"Compositions of account with id: {id}, hasn't been found in db.");
                    return NotFound(id);
                }

                logger.LogInformation($"Returned compositions of account with id: {id}");
                return compositions;
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetAllCompositionsOfAccount action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpPost]
        public async Task<ActionResult> CreateComposition([FromBody] CompositionViewModel composition)
        {
            try
            {         
                if (composition == null)
                {
                    logger.LogError($"Composition object sent from client is null");
                    return BadRequest("Composition object is null");
                }

                int id = await compositionAdapter.CreateComposition(composition).ConfigureAwait(false);
                logger.LogInformation($"Composition {composition.Title} is successfully added");
                return Ok(id);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside CreateComposition action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpPost("update")]
        public async Task<ActionResult> UpdateComposition([FromBody] CompositionViewModel composition)
        {
            try
            {
                if (composition == null)
                {
                    logger.LogError($"Composition object sent from client is null");
                    return BadRequest("Composition object is null");
                }

                await compositionAdapter.UpdateComposition(composition).ConfigureAwait(false);
                logger.LogInformation($"Composition {composition.Title} is successfully updated");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside UpdateComposition action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComposition(int id)
        {
            try
            {
                await compositionAdapter.DeleteComposition(id).ConfigureAwait(false);
                logger.LogInformation($"Composition with id: {id} is successfully deleted");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside DeleteComposition action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpPost("add-comment")]
        public async Task<ActionResult> CreateComment([FromBody] CompositionCommentsViewModel comment)
        {
            try
            {
                if (comment == null)
                {
                    logger.LogError($"Comment object sent from client is null");
                    return BadRequest("Comment object is null");
                }

                await commentsAdapter.AddComent(comment).ConfigureAwait(false);
                logger.LogInformation($"{comment.UserName} commentary is successfully added");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside CreateComment action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpPost("add-rating")]
        public async Task<ActionResult> CreateRating([FromBody] CompositionRatingViewModel rating)
        {
            try
            {
                if (rating == null)
                {
                    logger.LogError($"Rating object sent from client is null");
                    return BadRequest("Rating object is null");
                }

                await ratingsAdapter.AddRating(rating).ConfigureAwait(false);
                logger.LogInformation($"Rating for composition with id: {rating.CompositionId} is successfully added");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside CreateRating action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpPost("delete-images")]
        public async Task<ActionResult> DeleteImages(CloudImageViewModel[] images)
        {
            try
            {
                if (images == null)
                {
                    logger.LogError($"Images object sent from client is null");
                    return BadRequest("Images object is null");
                }

                await storageService.DeleteImagesRange(images).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside DeleteImages action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }
    }
}
