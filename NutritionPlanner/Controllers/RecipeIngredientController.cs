using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeIngredientsController : ControllerBase
    {
        private readonly IRecipeIngredientService _recipeIngredientService;

        public RecipeIngredientsController(IRecipeIngredientService recipeIngredientService)
        {
            _recipeIngredientService = recipeIngredientService;
        }

        [HttpGet("{recipeId}")]
        public async Task<ActionResult<List<RecipeIngredient>>> GetByRecipeId(int recipeId)
        {
            var ingredients = await _recipeIngredientService.GetByRecipeIdAsync(recipeId);
            return Ok(ingredients);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddRecipeIngredient(RecipeIngredient ingredient)
        {
            var ingredientId = await _recipeIngredientService.CreateRecipeIngredientAsync(ingredient);
            return CreatedAtAction(nameof(GetByRecipeId), new { recipeId = ingredient.RecipeId }, ingredientId);
        }
    }
}
