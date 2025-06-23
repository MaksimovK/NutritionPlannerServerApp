using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.DataAccess.Repositories.Interfaces
{
    public interface IUserProgressRepository
    {
        Task<int> CreateAsync(UserProgressEntity userProgress);
        Task DeleteAsync(int id);
        Task<List<UserProgressEntity>> GetProgressByUserIdAsync(Guid userId);
        Task UpdateAsync(UserProgressEntity userProgress);
        Task<bool> ExistsAsync(Guid userId, DateOnly date);
        Task<UserProgressEntity> GetProgressByUserIdAndDateAsync(Guid userId, DateOnly date);
    }
}