using Microsoft.AspNetCore.Mvc;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealTimesController : ControllerBase
    {
        private readonly IMealTimeService _mealTimeService;

        public MealTimesController(IMealTimeService mealTimeService)
        {
            _mealTimeService = mealTimeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MealTime>>> GetAllMealTimes()
        {
            var mealTimes = await _mealTimeService.GetAllAsync();

            var response = new
            {
                times = mealTimes
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateMealTime([FromBody] MealTime mealTime)
        {
            var mealTimeId = await _mealTimeService.CreateMealTimeAsync(mealTime);
            return CreatedAtAction(nameof(GetAllMealTimes), new { id = mealTimeId }, mealTimeId);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateMealTime(int id, [FromBody] MealTime mealTime)
        {
            mealTime.Id = id;
            await _mealTimeService.UpdateMealTimeAsync(mealTime);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMealTime(int id)
        {
            await _mealTimeService.DeleteMealTimeAsync(id);
            return NoContent();
        }

    }
}
