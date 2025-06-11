using Microsoft.AspNetCore.Mvc;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            return Ok(user);
        }


        [HttpPost]
        public async Task<ActionResult<int>> CreateUser(User user)
        {
            var userId = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = userId }, userId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(Guid id, User user)
        {
            if (id != user.Id)
                return BadRequest("User ID mismatch");
            try
            {
                await _userService.UpdateUserAsync(user);
                return NoContent();
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
                return StatusCode(500, new { Message = "An unexpected error occurred", Details = ex.Message });
            }
        }

        [HttpPut("{id}/role")]
        public async Task<ActionResult> UpdateUserRole(Guid id, [FromBody] RoleUpdateRequest request)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { Message = "User not found" });
                }

                user.Role = request.NewRole;
                await _userService.UpdateUserAsync(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        public class RoleUpdateRequest
        {
            public Role NewRole { get; set; }
        }
    }
}
