using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using MordorFanficWeb.PresentationAdapters.ChapterAdapter;
using MordorFanficWeb.ViewModels.ChapterViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using MordorFanficWeb.PresentationAdapters.ChapterLikesAdapter;
using MordorFanficWeb.ViewModels.ChapterLikesViewModels;

namespace MordorFanficWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChapterController : Controller
    {
        private readonly IChapterAdapter chapterAdapter;
        private readonly IChapterLikesAdapter likesAdapter;
        private readonly ILogger<ChapterController> logger;

        public ChapterController(IChapterAdapter chapterAdapter, IChapterLikesAdapter likesAdapter, ILogger<ChapterController> logger)
        {
            this.chapterAdapter = chapterAdapter;
            this.likesAdapter = likesAdapter;
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

        [HttpGet]
        public async Task<ActionResult<List<ChapterViewModel>>> GetAllChapters()
        {
            try
            {
                var chapters = await chapterAdapter.GetAllChapters().ConfigureAwait(false);
                if (chapters == null)
                {
                    logger.LogError($"Chapters list cannot be found.");
                    return NotFound();
                }

                logger.LogInformation("Returned all chapters from db");
                return chapters;
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetAllChapters action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers")]
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

        [Authorize(Policy = "RegisteredUsers")]
        [HttpPost("{id}")]
        public async Task<ActionResult> UpdateChapter([FromBody] ChapterViewModel chapter)
        {
            try
            {
                if (chapter == null)
                {
                    logger.LogError($"Chapter object sent from client is null");
                    return BadRequest("Chapter object is null");
                }

                await chapterAdapter.UpdateChapter(chapter).ConfigureAwait(false);
                logger.LogInformation($"Chapter {chapter.ChapterTitle} is successfully updated");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside UpdateChapter action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteChapter(int id)
        {
            try
            {
                await chapterAdapter.DeleteChapter(id).ConfigureAwait(false);
                logger.LogInformation($"Chapter with id: {id} is successfully deleted");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside DeleteChapter action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpPost("add-like")]
        public async Task<ActionResult> AddLike([FromBody] ChapterLikeViewModel like)
        {
            try
            {
                if (like == null)
                {
                    logger.LogError($"Like object sent from client is null");
                    return BadRequest("Like object is null");
                }

                await likesAdapter.AddLike(like).ConfigureAwait(false);
                logger.LogInformation($"Like added to the chapter with id: {like.ChapterId} is successfully added");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside AddLike action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
