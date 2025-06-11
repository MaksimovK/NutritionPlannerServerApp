using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.DataAccess.Repositories.Interfaces
{
    public interface IActivityLevelRepository
    {
        Task CreateAsync(ActivityLevelEntity activityLevel);
        Task DeleteAsync(int id);
        Task<List<ActivityLevelEntity>> GetAllAsync();
        Task<ActivityLevelEntity> GetByIdAsync(int id);
        Task UpdateAsync(ActivityLevelEntity activityLevel);
    }
}