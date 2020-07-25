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
        [HttpPost("update")]
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

        [Authorize(Policy = "RegisteredUsers")]
        [HttpPost("update-numeration")]
        public async Task<ActionResult> UpdateNumeration([FromBody] ChapterNumerationViewModel[] numeration)
        {
            try
            {
                if (numeration == null)
                {
                    logger.LogError($"Numeration object sent from client is null");
                    return BadRequest("Numeration object is null");
                }

                await chapterAdapter.UpdateChapterNumeration(numeration).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside UpdateChapter action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
