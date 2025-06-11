using Microsoft.EntityFrameworkCore;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;

namespace NutritionPlanner.DataAccess.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly NutritionPlannerDbContext _context;

        public AuthRepository(NutritionPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Guid> CreateUserAsync(UserEntity user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }
    }
}
