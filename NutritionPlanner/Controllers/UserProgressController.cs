using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionPlanner.Application.Services;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.API.Controllers
{
    [Authorize]
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
            try
            {
                var progressId = await _userProgressService.AddUserProgressAsync(progress);

                // Возвращаем 200 OK при обновлении и 201 Created при создании
                var existing = await _userProgressService.GetProgressByUserIdAndDateAsync(progress.UserId, progress.Date);
                if (existing != null && existing.Id == progressId)
                {
                    return Ok(progressId);
                }

                return CreatedAtAction(
                    nameof(GetProgressByUserIdAndDate),
                    new { userId = progress.UserId, date = progress.Date },
                    progressId
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
