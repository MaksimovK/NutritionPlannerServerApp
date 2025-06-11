using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.DataAccess.Repositories.Interfaces
{
    public interface IUserGoalRepository
    {
        Task CreateAsync(UserGoalEntity userGoal);
        Task DeleteAsync(int id);
        Task<List<UserGoalEntity>> GetAllAsync();
        Task<UserGoalEntity> GetByUserIdAsync(Guid userId);
        Task UpdateAsync(UserGoalEntity userGoal);
        Task<List<UserGoalEntity>> GetByUserIdAsyncList(Guid userId);
    }
}