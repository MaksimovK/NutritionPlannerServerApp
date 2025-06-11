using NutritionPlanner.Core.Models;

namespace NutritionPlanner.Application.Services.Interfaces
{
    public interface IMealPlanService
    {
        Task<int> CreateMealPlanAsync(MealPlan mealPlan);
        Task<MealPlan> GetMealPlanByUserIdAndDateAsync(Guid userId, DateOnly date);
        Task<List<MealPlan>> GetMealPlansByUserIdAndDateRangeAsync(Guid userId, DateOnly startDate, DateOnly endDate);
    }
}