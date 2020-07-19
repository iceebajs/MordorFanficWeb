using Microsoft.AspNetCore.Mvc;
using MordorFanficWeb.PresentationAdapters.AccountAdapter;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MordorFanficWeb.ViewModels;
using System;
using System.Collections.Generic;
using MordorFanficWeb.Common;
using MordorFanficWeb.Common.Helper;
using Microsoft.AspNetCore.Authorization;
using MordorFanficWeb.PresentationAdapters.CompositionAdapter;
using MordorFanficWeb.ViewModels.CompositionViewModels;

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
    }
}
