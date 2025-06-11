using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.DataAccess.Repositories.Interfaces
{
    public interface IRecipeRepository
    {
        Task CreateAsync(RecipeEntity recipe);
        Task DeleteAsync(int id);
        Task<List<RecipeEntity>> GetAllAsync();
        Task<RecipeEntity> GetByIdAsync(int id);
        Task UpdateAsync(RecipeEntity recipe);
        Task<List<RecipeEntity>> SearchByNameAsync(string name);
        Task<List<RecipeEntity>> GetUnapprovedAsync();
        Task ApproveAsync(int id);
    }
}