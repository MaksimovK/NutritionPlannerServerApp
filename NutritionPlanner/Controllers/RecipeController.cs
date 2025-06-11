using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.Core.DTO.NutritionPlanner.Core.Models;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.API.Controllers
{
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
         public async Task<ActionResult<List<RecipeWithNutritionDto>>> GetAllRecipes()
        {
            var recipes = await _recipeService.GetAllAsync();
            return Ok(recipes);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Recipe>>> SearchRecipes(string name)
        {
            var recipes = await _recipeService.SearchByNameAsync(name);
            return Ok(recipes);
        }

        [HttpPost]
        public async Task<ActionResult<Recipe>> CreateRecipe(
            [FromBody] RecipeCreateDto recipeDto)
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
    }
}