using NutritionPlanner.Core.Models;

namespace NutritionPlanner.Application.Services.Interfaces
{
    public interface IMealTimeService
    {
        Task<List<MealTime>> GetAllAsync();
        Task<int> CreateMealTimeAsync(MealTime mealTime);
        Task UpdateMealTimeAsync(MealTime mealTime);
        Task DeleteMealTimeAsync(int id);
    }
}