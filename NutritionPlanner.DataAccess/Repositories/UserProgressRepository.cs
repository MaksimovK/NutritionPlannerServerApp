using Microsoft.EntityFrameworkCore;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;

namespace NutritionPlanner.DataAccess.Repositories
{
    public class UserProgressRepository : IUserProgressRepository
    {
        private readonly NutritionPlannerDbContext _context;

        public UserProgressRepository(NutritionPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserProgressEntity>> GetProgressByUserIdAsync(Guid userId)
        {
            return await _context.UserProgress
                .Where(up => up.UserId == userId)
                .ToListAsync();
        }

        public async Task<UserProgressEntity> GetProgressByUserIdAndDateAsync(Guid userId, DateOnly date)
        {
            return await _context.UserProgress
                .FirstOrDefaultAsync(up => up.UserId == userId && up.Date == date);  
        }

        public async Task<int> CreateAsync(UserProgressEntity userProgress)
        {
            await _context.UserProgress.AddAsync(userProgress);
            await _context.SaveChangesAsync();
            return userProgress.Id;  
        }


        public async Task UpdateAsync(UserProgressEntity userProgress)
        {
            _context.UserProgress.Update(userProgress);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var userProgress = await _context.UserProgress.FindAsync(id);
            if (userProgress != null)
            {
                _context.UserProgress.Remove(userProgress);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid userId, DateOnly date)
        {
            return await _context.UserProgress
                .AnyAsync(up => up.UserId == userId && up.Date == date);
        }
    }
}
