using Microsoft.AspNetCore.Mvc;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealPlanItemsController : ControllerBase
    {
        private readonly IMealPlanItemService _mealPlanItemService;

        public MealPlanItemsController(IMealPlanItemService mealPlanItemService)
        {
            _mealPlanItemService = mealPlanItemService;
        }

        [HttpGet("{mealPlanId}")]
        public async Task<ActionResult<List<MealPlanItem>>> GetByMealPlanId(int mealPlanId)
        {
            var mealPlanItems = await _mealPlanItemService.GetByMealPlanIdAsync(mealPlanId);
            return Ok(mealPlanItems);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddMealPlanItem(MealPlanItem mealPlanItem)
        {
            // Проверяем корректность переданных данных
            if (mealPlanItem.MealPlanId <= 0)
            {
                return BadRequest("Неверный MealPlanId.");
            }

            try
            {
                // Добавляем MealPlanItem через сервис
                var mealPlanItemId = await _mealPlanItemService.AddMealPlanItemAsync(mealPlanItem);

                return CreatedAtAction(nameof(GetByMealPlanId), new { mealPlanId = mealPlanItem.MealPlanId }, mealPlanItemId);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMealPlanItem(int id, MealPlanItem mealPlanItem)
        {
            if (id != mealPlanItem.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                await _mealPlanItemService.UpdateMealPlanItemAsync(mealPlanItem);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMealPlanItem(int id)
        {
            try
            {
                await _mealPlanItemService.DeleteMealPlanItemAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("recipe")]
        public async Task<ActionResult<int>> AddRecipeToMealPlan([FromBody] AddRecipeToMealPlanRequest request)
        {
            try
            {
                var mealPlanItem = new MealPlanItem
                {
                    MealPlanId = request.MealPlanId,
                    MealTimeId = request.MealTimeId,
                    RecipeId = request.RecipeId,
                    Amount = request.Amount
                };

                var mealPlanItemId = await _mealPlanItemService.AddMealPlanItemAsync(mealPlanItem);
                return CreatedAtAction(nameof(GetByMealPlanId),
                    new { mealPlanId = request.MealPlanId },
                    mealPlanItemId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class AddRecipeToMealPlanRequest
        {
            public int MealPlanId { get; set; }
            public int MealTimeId { get; set; }
            public int RecipeId { get; set; }
            public decimal Amount { get; set; }
        }

    }
}