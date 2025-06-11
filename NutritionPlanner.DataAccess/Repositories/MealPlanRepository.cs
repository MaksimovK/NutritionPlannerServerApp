using Microsoft.EntityFrameworkCore;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;

namespace NutritionPlanner.DataAccess.Repositories
{
    public class MealPlanRepository : IMealPlanRepository
    {
        private readonly NutritionPlannerDbContext _context;

        public MealPlanRepository(NutritionPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<MealPlanEntity> GetByUserIdAsync(Guid userId)
        {
            return await _context.MealPlans
                .FirstOrDefaultAsync(mp => mp.UserId == userId);
        }

        public async Task<List<MealPlanEntity>> GetMealPlansByUserIdAndDateRangeAsync(Guid userId, DateOnly startDate, DateOnly endDate)
        {
            return await _context.MealPlans
                .Include(mp => mp.MealPlanItems)
                .Where(mp => mp.UserId == userId && mp.Date >= startDate && mp.Date <= endDate)
                .ToListAsync();
        }

        public async Task<MealPlanEntity> GetMealPlanByUserIdAndDateAsync(Guid userId, DateOnly date)
        {
            return await _context.MealPlans
                .Include(mp => mp.MealPlanItems)
                .FirstOrDefaultAsync(mp => mp.UserId == userId && mp.Date == date);
        }

        public async Task<List<MealPlanEntity>> GetAllAsync()
        {
            return await _context.MealPlans.ToListAsync();
        }

        public async Task<MealPlanEntity> GetMealPlanByIdAsync(int mealPlanId)
        {
            return await _context.MealPlans
                .FirstOrDefaultAsync(mp => mp.Id == mealPlanId);
        }


        public async Task CreateAsync(MealPlanEntity mealPlan)
        {
            await _context.MealPlans.AddAsync(mealPlan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MealPlanEntity mealPlan)
        {
            _context.MealPlans.Update(mealPlan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var mealPlan = await GetByUserIdAsync(id);
            if (mealPlan != null)
            {
                _context.MealPlans.Remove(mealPlan);
                await _context.SaveChangesAsync();
            }
        }
    }
}
