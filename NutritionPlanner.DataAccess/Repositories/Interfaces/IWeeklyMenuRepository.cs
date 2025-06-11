using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.DataAccess.Repositories.Interfaces
{
    public interface IWeeklyMenuRepository
    {
        Task<int> CreateAsync(WeeklyMenuEntity weeklyMenu);
        Task DeleteAsync(int id);
        Task<List<WeeklyMenuEntity>> GetByGoalTypeIdAsync(int goalTypeId);
        Task UpdateAsync(WeeklyMenuEntity weeklyMenu);
    }
}