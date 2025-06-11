using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.DataAccess.Repositories.Interfaces
{
    public interface IGoalTypeRepository
    {
        Task CreateAsync(GoalTypeEntity goalType);
        Task DeleteAsync(int id);
        Task<List<GoalTypeEntity>> GetAllAsync();
        Task<GoalTypeEntity> GetByIdAsync(int id);
        Task UpdateAsync(GoalTypeEntity goalType);
    }
}