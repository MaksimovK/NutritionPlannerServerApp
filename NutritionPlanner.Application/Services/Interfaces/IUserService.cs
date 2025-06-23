using NutritionPlanner.Core.Models;

namespace NutritionPlanner.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<Guid> CreateUserAsync(User user);
        Task<User> GetUserByIdAsync(Guid userId);
        Task UpdateUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task UpdateUserRoleAsync(Guid userId, Role newRole);
        Task UnblockUserAsync(Guid userId);
        Task BlockUserAsync(Guid userId, DateTime blockedUntil, string reason);
    }
}