using NutritionPlanner.Core.Models;

namespace NutritionPlanner.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<Guid> CreateUserAsync(User user);
        Task<User> GetUserByIdAsync(Guid userId);
        Task UpdateUserAsync(User user);
    }
}