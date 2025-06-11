using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.DataAccess.Repositories.Interfaces
{
    public interface IRecipeIngredientRepository
    {
        Task CreateAsync(RecipeIngredientEntity recipeIngredient);
        Task DeleteAsync(int id);
        Task<List<RecipeIngredientEntity>> GetByRecipeIdAsync(int recipeId);
        Task UpdateAsync(RecipeIngredientEntity recipeIngredient);
        Task CreateBatchAsync(IEnumerable<RecipeIngredientEntity> ingredients);
        Task<List<RecipeIngredientEntity>> GetByRecipeIdsAsync(IEnumerable<int> recipeIds);
        Task<List<RecipeIngredientEntity>> GetByRecipeIdsWithProductsAsync(IEnumerable<int> recipeIds);
    }
}