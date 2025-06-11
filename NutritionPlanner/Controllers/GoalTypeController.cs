using Microsoft.AspNetCore.Mvc;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalTypesController : ControllerBase
    {
        private readonly IGoalTypeService _goalTypeService;

        public GoalTypesController(IGoalTypeService goalTypeService)
        {
            _goalTypeService = goalTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GoalType>>> GetAllGoalTypes()
        {
            var goalTypes = await _goalTypeService.GetAllAsync();
            var response = new
            {
                types = goalTypes
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateGoalType([FromBody] GoalType goalType)
        {
            var goalTypeId = await _goalTypeService.CreateGoalTypeAsync(goalType);
            return CreatedAtAction(nameof(GetAllGoalTypes), new { id = goalTypeId }, goalTypeId);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateGoalType(int id, [FromBody] GoalType goalType)
        {
            goalType.Id = id;
            await _goalTypeService.UpdateGoalTypeAsync(goalType);
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteGoalType(int id)
        {
            await _goalTypeService.DeleteGoalTypeAsync(id);
            return NoContent();
        }
    }
}
