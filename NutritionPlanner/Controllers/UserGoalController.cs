using Microsoft.AspNetCore.Mvc;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGoalsController : ControllerBase
    {
        private readonly IUserGoalService _userGoalService;

        public UserGoalsController(IUserGoalService userGoalService)
        {
            _userGoalService = userGoalService;
        }

        public class UserGoalsResponse
        {
            public List<UserGoal> Goals { get; set; }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserGoalsResponse>> GetGoalsByUserId(Guid userId)
        {
            try
            {
                var goals = await _userGoalService.GetByUserIdAsync(userId);
                if (goals == null || !goals.Any())
                {
                    return NotFound("Цели пользователя не найдены");
                }

                var response = new UserGoalsResponse
                {
                    Goals = goals
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult> UpdateUserGoal([FromBody] UpdateUserGoalRequest request)
        {
            try
            {
                await _userGoalService.UpdateUserGoalAsync(request.UserId, request.GoalTypeId);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<ActionResult> CreateUserGoal(UserGoal userGoal)
        {
            await _userGoalService.CreateUserGoalAsync(userGoal);
            return CreatedAtAction(nameof(GetGoalsByUserId), new { userId = userGoal.UserId }, userGoal);
        }

        public class UpdateUserGoalRequest
        {
            public Guid UserId { get; set; }
            public int GoalTypeId { get; set; }
        }

    }
}
