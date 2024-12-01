using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.User
{
    [Authorize(Policy = "allLoggedPolicy")]
    [Route("api/userLevels")]
    public class UserLevelController : ControllerBase
    {
        private readonly IUserLevelService _userLevelService;

        public UserLevelController(IUserLevelService userLevelService)
        {
            _userLevelService = userLevelService;
        }

        // POST: api/UserLevel
        [HttpPost]
        public ActionResult<UserLevelDto> CreateUserLevel([FromBody] UserLevelDto userLevelDto)
        {
            var result = _userLevelService.CreateUserLevel(userLevelDto);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetUserLevelById), new { id = result.Value.Id }, result.Value);
            }
            else
            {
                return BadRequest(new { Message = "Failed to create user level", Details = result.Errors });
            }
        }

        // PUT: api/UserLevel/{id}
        [HttpPut]
        public ActionResult<UserLevelDto> UpdateUserLevel([FromBody] UserLevelDto userLevelDto)
        {
            var result = _userLevelService.UpdateUserLevel(userLevelDto);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return NotFound(new { Message = "User level not found", Details = result.Errors });
            }
        }

        // GET: api/UserLevel/{id}
        [HttpGet("{id}")]
        public ActionResult<UserLevelDto> GetUserLevelById(long id)
        {
            var result = _userLevelService.GetUserLevelById(id);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return NotFound(new { Message = "User level not found", Details = result.Errors });
            }
        }

        // GET: api/UserLevel
        [HttpGet]
        public ActionResult<List<UserLevelDto>> GetAllUserLevels()
        {
            var result = _userLevelService.GetAllUserLevels();

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return NotFound(new { Message = "No user levels found", Details = result.Errors });
            }
        }
    }
}
