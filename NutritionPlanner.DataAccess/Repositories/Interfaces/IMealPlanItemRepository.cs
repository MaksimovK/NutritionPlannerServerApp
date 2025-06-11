using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.DataAccess.Repositories.Interfaces
{
    public interface IMealPlanItemRepository
    {
        Task CreateAsync(MealPlanItemEntity mealPlanItem);
        Task DeleteAsync(int id);
        Task<List<MealPlanItemEntity>> GetByMealPlanIdAsync(int mealPlanId);
        Task UpdateAsync(MealPlanItemEntity mealPlanItem);
        Task<MealPlanItemEntity> GetByIdAsync(int id);
        Task<MealPlanEntity> GetMealPlanByIdAsync(int mealPlanId);
        Task UpdateMealPlanAsync(MealPlanEntity mealPlan);
        Task<ProductEntity> GetProductByIdAsync(int productId);
        Task<RecipeEntity> GetRecipeByIdAsync(int recipeId);
    }
}