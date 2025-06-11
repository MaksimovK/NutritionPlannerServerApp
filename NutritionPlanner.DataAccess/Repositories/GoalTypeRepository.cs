using Microsoft.EntityFrameworkCore;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;

namespace NutritionPlanner.DataAccess.Repositories
{
    public class GoalTypeRepository : IGoalTypeRepository
    {
        private readonly NutritionPlannerDbContext _context;

        public GoalTypeRepository(NutritionPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<GoalTypeEntity> GetByIdAsync(int id)
        {
            return await _context.GoalTypes
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<List<GoalTypeEntity>> GetAllAsync()
        {
            return await _context.GoalTypes
                .OrderBy(type => type.Id)
                .ToListAsync();
        }

        public async Task CreateAsync(GoalTypeEntity goalType)
        {
            await _context.GoalTypes.AddAsync(goalType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GoalTypeEntity goalType)
        {
            _context.GoalTypes.Update(goalType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var goalType = await GetByIdAsync(id);
            if (goalType != null)
            {
                _context.GoalTypes.Remove(goalType);
                await _context.SaveChangesAsync();
            }
        }
    }
}
