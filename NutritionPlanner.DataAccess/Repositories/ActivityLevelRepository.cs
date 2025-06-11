using Microsoft.EntityFrameworkCore;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;

namespace NutritionPlanner.DataAccess.Repositories
{
    public class ActivityLevelRepository : IActivityLevelRepository
    {
        private readonly NutritionPlannerDbContext _context;

        public ActivityLevelRepository(NutritionPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<ActivityLevelEntity> GetByIdAsync(int id)
        {
            return await _context.ActivityLevels
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<ActivityLevelEntity>> GetAllAsync()
        {
            return await _context.ActivityLevels
                .OrderBy(level => level.Id)
                .ToListAsync();
        }

        public async Task CreateAsync(ActivityLevelEntity activityLevel)
        {
            await _context.ActivityLevels.AddAsync(activityLevel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ActivityLevelEntity activityLevel)
        {
            _context.ActivityLevels.Update(activityLevel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var activityLevel = await GetByIdAsync(id);
            if (activityLevel != null)
            {
                _context.ActivityLevels.Remove(activityLevel);
                await _context.SaveChangesAsync();
            }
        }
    }
}
