using Microsoft.EntityFrameworkCore;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;

namespace NutritionPlanner.DataAccess.Repositories
{
    public class UserGoalRepository : IUserGoalRepository
    {
        private readonly NutritionPlannerDbContext _context;

        public UserGoalRepository(NutritionPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<UserGoalEntity> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserGoals
                .FirstOrDefaultAsync(ug => ug.UserId == userId);
        }

        public async Task<List<UserGoalEntity>> GetByUserIdAsyncList(Guid userId)
        {
            return await _context.UserGoals
                .Where(ug => ug.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<UserGoalEntity>> GetAllAsync()
        {
            return await _context.UserGoals.ToListAsync();
        }

        public async Task CreateAsync(UserGoalEntity userGoal)
        {
            await _context.UserGoals.AddAsync(userGoal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserGoalEntity userGoal)
        {
            _context.UserGoals.Update(userGoal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var userGoal = await _context.UserGoals.FindAsync(id);
            if (userGoal != null)
            {
                _context.UserGoals.Remove(userGoal);
                await _context.SaveChangesAsync();
            }
        }
    }
}
