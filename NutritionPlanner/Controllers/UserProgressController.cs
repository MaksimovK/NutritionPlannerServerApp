using Microsoft.AspNetCore.Mvc;
using NutritionPlanner.Application.Services;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProgressController : ControllerBase
    {
        private readonly IUserProgressService _userProgressService;

        public UserProgressController(IUserProgressService userProgressService)
        {
            _userProgressService = userProgressService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<UserProgress>>> GetProgressByUserId(Guid userId)
        {
            var progress = await _userProgressService.GetProgressByUserIdAsync(userId);
            return Ok(progress);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateUserProgress(UserProgress progress)
        {
            var progressId = await _userProgressService.AddUserProgressAsync(progress);
            return CreatedAtAction(nameof(GetProgressByUserId), new { userId = progress.UserId }, progressId);
        }

        [HttpGet("{userId}/{date}")]
        public async Task<ActionResult<UserProgress>> GetProgressByUserIdAndDate(Guid userId, DateOnly date)
        {
            var progress = await _userProgressService.GetProgressByUserIdAndDateAsync(userId, date);
            if (progress == null)
            {
                return NotFound();
            }
            return Ok(progress);
        }
    }
}
