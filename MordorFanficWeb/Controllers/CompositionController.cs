using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using MordorFanficWeb.PresentationAdapters.CompositionAdapter;
using MordorFanficWeb.ViewModels.CompositionViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace MordorFanficWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompositionController : Controller
    {
        private readonly ICompositionAdapter compositionAdapter;
        private readonly ILogger<CompositionController> logger;

        public CompositionController(ICompositionAdapter compositionAdapter, ILogger<CompositionController> logger)
        {
            this.compositionAdapter = compositionAdapter;
            this.logger = logger;
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

                await compositionAdapter.CreateComposition(composition).ConfigureAwait(false);
                logger.LogInformation($"Composition {composition.Title} is successfully added");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside CreateComposition action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpPost("{id}")]
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
    }
}
