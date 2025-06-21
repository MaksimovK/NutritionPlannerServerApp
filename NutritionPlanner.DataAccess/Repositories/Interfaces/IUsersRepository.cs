using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.DataAccess.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task CreateAsync(UserEntity user);
        Task DeleteAsync(Guid id);
        Task<UserEntity> GetByEmailAsync(string email);
        Task<UserEntity> GetByIdAsync(Guid id);
        Task UpdateAsync(UserEntity user);
        Task<IEnumerable<UserEntity>> GetByRoleAsync(Role role);
        Task<IEnumerable<UserEntity>> GetAllAsync();
    }
}