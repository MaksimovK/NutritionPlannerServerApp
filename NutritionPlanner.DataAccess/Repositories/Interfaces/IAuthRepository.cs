using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.DataAccess.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<UserEntity> GetUserByEmailAsync(string email);
        Task<Guid> CreateUserAsync(UserEntity user);
    }
}
