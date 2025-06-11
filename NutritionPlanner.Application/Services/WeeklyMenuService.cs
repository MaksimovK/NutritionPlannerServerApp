using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Repositories.Interfaces;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.Application.Services
{
    public class WeeklyMenuService : IWeeklyMenuService
    {
        private readonly IWeeklyMenuRepository _repository;

        public WeeklyMenuService(IWeeklyMenuRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<WeeklyMenu>> GetByGoalTypeIdAsync(int goalTypeId)
        {
            var weeklyMenus = await _repository.GetByGoalTypeIdAsync(goalTypeId);
            var result = new List<WeeklyMenu>();
            foreach (var menu in weeklyMenus)
            {
                result.Add(new WeeklyMenu
                {
                    Id = menu.Id,
                    GoalTypeId = menu.GoalTypeId,
                    DayOfWeek = menu.DayOfWeek,
                    MealTimeId = menu.MealTimeId,
                    RecipeId = menu.RecipeId,
                    ProductId = menu.ProductId,
                    Amount = menu.Amount
                });
            }
            return result;
        }

        public async Task<int> CreateWeeklyMenuAsync(WeeklyMenu menu)
        {
            var menuEntity = new WeeklyMenuEntity
            {
                Id = menu.Id,
                GoalTypeId = menu.GoalTypeId,
                DayOfWeek = menu.DayOfWeek,
                MealTimeId = menu.MealTimeId,
                RecipeId = menu.RecipeId,
                ProductId = menu.ProductId,
                Amount = menu.Amount
            };
            return await _repository.CreateAsync(menuEntity);
        }
    }
}
