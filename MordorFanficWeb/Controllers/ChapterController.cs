using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using MordorFanficWeb.PresentationAdapters.ChapterAdapter;
using MordorFanficWeb.ViewModels.ChapterViewModels;

namespace MordorFanficWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChapterController : Controller
    {
        private readonly IChapterAdapter chapterAdapter;
        private readonly ILogger<CompositionController> logger;

        public ChapterController(IChapterAdapter chapterAdapter, ILogger<CompositionController> logger)
        {
            this.chapterAdapter = chapterAdapter;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChapterViewModel>> GetChapterById(int id)
        {
            try
            {
                var chapter = await chapterAdapter.GetChapter(id).ConfigureAwait(false);

                if (chapter == null)
                {
                    logger.LogError($"Chapter with id: {id}, hasn't been found in db.");
                    return NotFound(id);
                }
                logger.LogInformation($"Returned chapter with id: {id}");
                return chapter;
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetChapterById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateChapter([FromBody] ChapterViewModel chapter)
        {
            try
            {
                if (chapter == null)
                {
                    logger.LogError($"Chapter object sent from client is null");
                    return BadRequest("Chapter object is null");
                }


                await chapterAdapter.CreateChapter(chapter).ConfigureAwait(false);
                logger.LogInformation($"Chapter {chapter.ChapterTitle} is successfully added");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside CreateChapter action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
