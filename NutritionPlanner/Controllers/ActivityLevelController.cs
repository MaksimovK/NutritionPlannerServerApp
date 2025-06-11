using Microsoft.AspNetCore.Mvc;
using NutritionPlanner.Application.Services;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityLevelsController : ControllerBase
    {
        private readonly IActivityLevelService _activityLevelService;

        public ActivityLevelsController(IActivityLevelService activityLevelService)
        {
            _activityLevelService = activityLevelService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActivityLevel>>> GetAllActivityLevels()
        {
            var activityLevels = await _activityLevelService.GetAllAsync();

            var response = new
            {
                levels = activityLevels
            };

            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateActivityLevel(int id, [FromBody] ActivityLevel activityLevel)
        {
            activityLevel.Id = id;
            await _activityLevelService.UpdateActivityLevelAsync(activityLevel);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateActivityLevel([FromBody] ActivityLevel activityLevel)
        {
            var activityLevelId = await _activityLevelService.CreateActivityLevelAsync(activityLevel);
            return CreatedAtAction(nameof(GetAllActivityLevels), new { id = activityLevelId }, activityLevelId);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteActivityLevel(int id)
        {
            await _activityLevelService.DeleteActivityLevelAsync(id);
            return NoContent();
        }

    }
}
