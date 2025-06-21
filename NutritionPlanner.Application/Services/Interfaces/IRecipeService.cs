using NutritionPlanner.Core.DTO.NutritionPlanner.Core.Models;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.Application.Services.Interfaces
{
    public interface IRecipeService
    {
        Task<List<RecipeWithNutritionDto>> GetAllAsync(RecipeFilter filter);
        Task<RecipeWithNutritionDto> GetByIdAsync(int id);
        Task<RecipeWithNutritionDto> CreateRecipeAsync(RecipeDto recipeDto);
        Task<List<RecipeWithNutritionDto>> SearchByNameAsync(string name, RecipeFilter filter);
        Task<List<RecipeWithNutritionDto>> GetUnapprovedRecipesAsync();
        Task<List<RecipeWithNutritionDto>> GetByIdsAsync(List<int> ids);
        Task ApproveRecipeAsync(int id);
        Task DeleteRecipeAsync(int id);
    }
}