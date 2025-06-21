using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.Core.DTO.NutritionPlanner.Core.Models;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipesController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeWithNutritionDto>> GetById(int id)
        {
            var recipe = await _recipeService.GetByIdAsync(id);
            if (recipe == null) return NotFound();
            return Ok(recipe);
        }

        [HttpGet]
        public async Task<ActionResult<List<RecipeWithNutritionDto>>> GetAllRecipes(
           [FromQuery] bool? highProtein = null,
           [FromQuery] bool? lowCalorie = null,
           [FromQuery] bool? highCalorie = null,
           [FromQuery] bool? lowCarb = null,
           [FromQuery] bool? highCarb = null,
           [FromQuery] bool? lowFat = null,
           [FromQuery] bool? highFat = null)
        {
            var filter = new RecipeFilter
            {
                HighProtein = highProtein,
                LowCalorie = lowCalorie,
                HighCalorie = highCalorie,
                LowCarb = lowCarb,
                HighCarb = highCarb,
                LowFat = lowFat,
                HighFat = highFat
            };

            var recipes = await _recipeService.GetAllAsync(filter);
            return Ok(recipes);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<RecipeWithNutritionDto>>> SearchRecipes(
         [FromQuery] string name,
         [FromQuery] bool? highProtein = null,
         [FromQuery] bool? lowCalorie = null,
         [FromQuery] bool? highCalorie = null,
         [FromQuery] bool? lowCarb = null,
         [FromQuery] bool? highCarb = null,
         [FromQuery] bool? lowFat = null,
         [FromQuery] bool? highFat = null)
        {
            var filter = new RecipeFilter
            {
                HighProtein = highProtein,
                LowCalorie = lowCalorie,
                HighCalorie = highCalorie,
                LowCarb = lowCarb,
                HighCarb = highCarb,
                LowFat = lowFat,
                HighFat = highFat
            };

            var recipes = await _recipeService.SearchByNameAsync(name, filter);
            return Ok(recipes);
        }

        [HttpPost]
        public async Task<ActionResult<Recipe>> CreateRecipe(
            [FromBody] RecipeDto recipeDto)
        {
            if (recipeDto == null)
            {
                return BadRequest("Invalid request data");
            }

            try
            {
                var createdRecipe = await _recipeService.CreateRecipeAsync(recipeDto);
                return CreatedAtAction(nameof(GetById),
                    new { id = createdRecipe.Id },
                    createdRecipe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            await _recipeService.DeleteRecipeAsync(id);
            return NoContent();
        }

        [HttpGet("unapproved")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<RecipeWithNutritionDto>>> GetUnapproved()
        {
            var list = await _recipeService.GetUnapprovedRecipesAsync();
            return Ok(list);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            await _recipeService.ApproveRecipeAsync(id);
            return NoContent();
        }

        [HttpPost("by-ids")]
        public async Task<ActionResult<List<RecipeWithNutritionDto>>> GetRecipesByIds([FromBody] List<int> ids)
        {
            var recipes = await _recipeService.GetByIdsAsync(ids);
            return Ok(recipes);
        }
    }
}