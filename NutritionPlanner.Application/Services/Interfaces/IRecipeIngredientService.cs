using NutritionPlanner.Core.Models;

namespace NutritionPlanner.Application.Services.Interfaces
{
    public interface IRecipeIngredientService
    {
        Task<int> CreateRecipeIngredientAsync(RecipeIngredient ingredient);
        Task<List<RecipeIngredient>> GetByRecipeIdAsync(int recipeId);
    }
}