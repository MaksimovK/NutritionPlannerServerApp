using NutritionPlanner.Core.Models;

namespace NutritionPlanner.Application.Services
{
    public interface IActivityLevelService
    {
        Task<List<ActivityLevel>> GetAllAsync();
        Task<ActivityLevel> GetByIdAsync(int id);
        Task DeleteActivityLevelAsync(int id);
        Task<int> CreateActivityLevelAsync(ActivityLevel activityLevel);
        Task UpdateActivityLevelAsync(ActivityLevel activityLevel);
    }
}