using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.DataAccess.Repositories.Interfaces
{
    public interface IMealTimeRepository
    {
        Task CreateAsync(MealTimeEntity mealTime);
        Task DeleteAsync(int id);
        Task<List<MealTimeEntity>> GetAllAsync();
        Task<MealTimeEntity> GetByIdAsync(int id);
        Task UpdateAsync(MealTimeEntity mealTime);
    }
}