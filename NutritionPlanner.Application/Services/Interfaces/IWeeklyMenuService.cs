using NutritionPlanner.Core.Models;

namespace NutritionPlanner.Application.Services.Interfaces
{
    public interface IWeeklyMenuService
    {
        Task<int> CreateWeeklyMenuAsync(WeeklyMenu menu);
        Task<List<WeeklyMenu>> GetByGoalTypeIdAsync(int goalTypeId);
    }
}