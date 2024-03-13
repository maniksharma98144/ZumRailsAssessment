using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Dtos;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/v{version:apiVersion}/posts")]
    public class PostController : Controller
    {
        private readonly ILoggerService _logger;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostController(ILoggerService logger, IPostService logic, IMapper mapper)
        {
            _logger = logger;
            _postService = logic;
            _mapper = mapper;
        }

        /// <summary>
        /// Get posts
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(List<PostDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts(
            [Required]
            [FromQuery] string tags,
            [FromQuery] string sortBy = "Id",
            [FromQuery] string direction = "asc"
            )
        {
            try
            {
                _logger.LogInfo("Post Controller Called");
                var posts = await _postService.GetPosts(tags.Split(","), sortBy, direction);
                if (posts == null)
                {
                    _logger.LogError($"No posts found for tags{tags}");
                    return NotFound("No posts found");
                }

                var postsDto = _mapper.Map<List<PostDto>>(posts);

                return Ok(postsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error");
                return BadRequest(ex.Message);
            }

        }
    }
}

