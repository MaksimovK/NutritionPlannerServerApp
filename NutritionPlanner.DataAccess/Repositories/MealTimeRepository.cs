using Microsoft.EntityFrameworkCore;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;

namespace NutritionPlanner.DataAccess.Repositories
{
    public class MealTimeRepository : IMealTimeRepository
    {
        private readonly NutritionPlannerDbContext _context;

        public MealTimeRepository(NutritionPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<MealTimeEntity> GetByIdAsync(int id)
        {
            return await _context.MealTimes
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<MealTimeEntity>> GetAllAsync()
        {
            return await _context.MealTimes
                .OrderBy(time => time.Id)
                .ToListAsync();
        }

        public async Task CreateAsync(MealTimeEntity mealTime)
        {
            await _context.MealTimes.AddAsync(mealTime);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MealTimeEntity mealTime)
        {
            _context.MealTimes.Update(mealTime);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var mealTime = await GetByIdAsync(id);
            if (mealTime != null)
            {
                _context.MealTimes.Remove(mealTime);
                await _context.SaveChangesAsync();
            }
        }
    }
}
