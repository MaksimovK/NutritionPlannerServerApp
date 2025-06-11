using Microsoft.EntityFrameworkCore;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;

namespace NutritionPlanner.DataAccess.Repositories
{
    public class WeeklyMenuRepository : IWeeklyMenuRepository
    {
        private readonly NutritionPlannerDbContext _context;

        public WeeklyMenuRepository(NutritionPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<List<WeeklyMenuEntity>> GetByGoalTypeIdAsync(int goalTypeId)
        {
            return await _context.WeeklyMenus
                .Where(wm => wm.GoalTypeId == goalTypeId)
                .ToListAsync();
        }

        public async Task<int> CreateAsync(WeeklyMenuEntity weeklyMenu)
        {
            await _context.WeeklyMenus.AddAsync(weeklyMenu);
            await _context.SaveChangesAsync();
            return weeklyMenu.Id;
        }


        public async Task UpdateAsync(WeeklyMenuEntity weeklyMenu)
        {
            _context.WeeklyMenus.Update(weeklyMenu);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var weeklyMenu = await _context.WeeklyMenus.FindAsync(id);
            if (weeklyMenu != null)
            {
                _context.WeeklyMenus.Remove(weeklyMenu);
                await _context.SaveChangesAsync();
            }
        }
    }
}
