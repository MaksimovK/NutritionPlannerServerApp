using NutritionPlanner.Core.Models;

namespace NutritionPlanner.Application.Services.Interfaces
{
    public interface IGoalTypeService
    {
        Task<List<GoalType>> GetAllAsync();
        Task<int> CreateGoalTypeAsync(GoalType goalType);
        Task DeleteGoalTypeAsync(int id);
        Task UpdateGoalTypeAsync(GoalType goalType);
    }
}
