using NutritionPlanner.Core.DTO.NutritionPlanner.Core.Models;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.Application.Services.Interfaces
{
    public interface IRecipeService
    {
        Task<List<RecipeWithNutritionDto>> GetAllAsync();
        Task<RecipeWithNutritionDto> GetByIdAsync(int id);
        Task<RecipeWithNutritionDto> CreateRecipeAsync(RecipeCreateDto recipeDto);
        Task<List<RecipeWithNutritionDto>> SearchByNameAsync(string name);
        Task<List<RecipeWithNutritionDto>> GetUnapprovedRecipesAsync();
        Task ApproveRecipeAsync(int id);
        Task DeleteRecipeAsync(int id);
    }
}