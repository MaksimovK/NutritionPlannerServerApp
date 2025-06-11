using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.DataAccess.Repositories.Interfaces
{
    public interface IMealPlanRepository
    {
        Task CreateAsync(MealPlanEntity mealPlan);
        Task DeleteAsync(Guid id);
        Task<List<MealPlanEntity>> GetAllAsync();
        Task<MealPlanEntity> GetByUserIdAsync(Guid userId);
        Task<MealPlanEntity> GetMealPlanByUserIdAndDateAsync(Guid userId, DateOnly date);
        Task UpdateAsync(MealPlanEntity mealPlan);
        Task<MealPlanEntity> GetMealPlanByIdAsync(int mealPlanId);
        Task<List<MealPlanEntity>> GetMealPlansByUserIdAndDateRangeAsync(Guid userId, DateOnly startDate, DateOnly endDate);
    }
}