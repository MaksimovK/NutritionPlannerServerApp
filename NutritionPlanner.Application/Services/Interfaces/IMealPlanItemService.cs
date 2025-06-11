using NutritionPlanner.Core.Models;

namespace NutritionPlanner.Application.Services.Interfaces
{
    public interface IMealPlanItemService
    {
        Task<int> AddMealPlanItemAsync(MealPlanItem mealPlanItem);
        Task<List<MealPlanItem>> GetByMealPlanIdAsync(int mealPlanId);  
        Task UpdateMealPlanItemAsync(MealPlanItem mealPlanItem);
        Task DeleteMealPlanItemAsync(int id);
    }
}