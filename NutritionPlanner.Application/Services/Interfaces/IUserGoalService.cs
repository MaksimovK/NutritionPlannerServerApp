using NutritionPlanner.Core.Models;

namespace NutritionPlanner.Application.Services.Interfaces
{
    public interface IUserGoalService
    {
        Task<List<UserGoal>> GetByUserIdAsync(Guid userId);
        Task CreateUserGoalAsync(UserGoal userGoal);
        Task UpdateUserGoalAsync(Guid userId, int goalTypeId);
    }
}