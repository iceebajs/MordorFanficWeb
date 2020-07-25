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
using MordorFanficWeb.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MordorFanficWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChapterController : Controller
    {
        private readonly IChapterAdapter chapterAdapter;
        private readonly IChapterLikesAdapter likesAdapter;
        private readonly ILogger<ChapterController> logger;
        private readonly ICloudStorageService storageService;

        public ChapterController(IChapterAdapter chapterAdapter, IChapterLikesAdapter likesAdapter, ILogger<ChapterController> logger, ICloudStorageService storageService)
        {
            this.chapterAdapter = chapterAdapter;
            this.likesAdapter = likesAdapter;
            this.logger = logger;
            this.storageService = storageService;
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
                logger.LogError($"Something went wrong inside UpdateNumeration action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers"), DisableRequestSizeLimit]
        [HttpPost("upload-image")]
        public async Task<ActionResult<CloudImageViewModel>> UploadImage()
        {
            try
            {
                var file = Request.Form.Files[0];
                if (file == null)
                {
                    logger.LogError($"File object sent from client is null");
                    return BadRequest("File object is null");
                }
                 
                return await storageService.UploadAsync(file).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside UploadImage action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "RegisteredUsers")]
        [HttpPost("delete-image")]
        public async Task<ActionResult> DeleteImage(CloudImageViewModel image)
        {
            try
            {
                if (image == null)
                {
                    logger.LogError($"Image object sent from client is null");
                    return BadRequest("Image object is null");
                }

                await storageService.DeleteImage(image.Url).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside DeleteImage action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }
    }
}
