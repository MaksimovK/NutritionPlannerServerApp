using NutritionPlanner.Core.Models;

namespace NutritionPlanner.Application.Services
{
    public interface IUserProgressService
    {
        Task<int> AddUserProgressAsync(UserProgress progress);
        Task<List<UserProgress>> GetProgressByUserIdAsync(Guid userId);
        Task<UserProgress> GetProgressByUserIdAndDateAsync(Guid userId, DateOnly date);
    }
}