using Microsoft.AspNetCore.Mvc;
using NutritionPlanner.Application.Services;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.Core.Models;
using NutritionPlanner.Application.Utilities;

namespace NutritionPlanner.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserGoalService _userGoalService;
        private readonly INutrition _nutritionService;

        public AuthController(IAuthService authService, IUserGoalService userGoalService, INutrition nutritionService)
        {
            _authService = authService;
            _userGoalService = userGoalService;
            _nutritionService = nutritionService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                var userId = await _authService.RegisterUserAsync(registerRequest);

                var calories = _nutritionService.CalculateCalories(registerRequest.Weight, registerRequest.Height, registerRequest.Age, registerRequest.Gender, registerRequest.ActivityLevelId, registerRequest.GoalTypeId);
                var bju = _nutritionService.CalculateBJU(calories, registerRequest.Weight, registerRequest.GoalTypeId);

                var userGoals = new UserGoal
                {
                    UserId = userId,
                    GoalTypeId = registerRequest.GoalTypeId,
                    Calories = calories,
                    Protein = bju.Protein,
                    Fat = bju.Fat,
                    Carbohydrates = bju.Carbohydrates,
                    CreatedAt = DateTime.UtcNow
                };

                await _userGoalService.CreateUserGoalAsync(userGoals);

                var token = await _authService.AuthenticateUserAsync(registerRequest.Email, registerRequest.Password);
                var user = await _authService.GetUserByEmailAsync(registerRequest.Email);

                return Ok(new
                {
                    UserId = userId,
                    Token = token,
                    UserRole = user.Role.ToString() 
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var token = await _authService.AuthenticateUserAsync(loginRequest.Email, loginRequest.Password);
                var user = await _authService.GetUserByEmailAsync(loginRequest.Email);

                return Ok(new
                {
                    Token = token,
                    UserId = user.Id,
                    UserRole = user.Role.ToString()
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
