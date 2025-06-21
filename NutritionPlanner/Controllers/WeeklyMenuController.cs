using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WeeklyMenusController : ControllerBase
    {
        private readonly IWeeklyMenuService _weeklyMenuService;

        public WeeklyMenusController(IWeeklyMenuService weeklyMenuService)
        {
            _weeklyMenuService = weeklyMenuService;
        }

        [HttpGet("{goalTypeId}")]
        public async Task<ActionResult<List<WeeklyMenu>>> GetByGoalTypeId(int goalTypeId)
        {
            var weeklyMenus = await _weeklyMenuService.GetByGoalTypeIdAsync(goalTypeId);
            return Ok(weeklyMenus);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateWeeklyMenu(WeeklyMenu menu)
        {
            var menuId = await _weeklyMenuService.CreateWeeklyMenuAsync(menu);
            return CreatedAtAction(nameof(GetByGoalTypeId), new { goalTypeId = menu.GoalTypeId }, menuId);
        }
    }
}
