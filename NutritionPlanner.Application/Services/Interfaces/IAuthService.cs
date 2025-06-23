using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> AuthenticateUserAsync(string login, string password);
        Task<Guid> RegisterUserAsync(RegisterRequest request);
        Task<UserEntity> GetUserByEmailAsync(string email);

    }
}
