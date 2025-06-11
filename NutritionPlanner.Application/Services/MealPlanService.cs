using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Repositories.Interfaces;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.Application.Services
{
    public class MealPlanService : IMealPlanService
    {
        private readonly IMealPlanRepository _repository;

        public MealPlanService(IMealPlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<MealPlan> GetMealPlanByUserIdAndDateAsync(Guid userId, DateOnly date)
        {
            var mealPlanEntity = await _repository.GetMealPlanByUserIdAndDateAsync(userId, date);

            if (mealPlanEntity == null) return null;

            if (mealPlanEntity.MealPlanItems == null || !mealPlanEntity.MealPlanItems.Any())
            {
                mealPlanEntity.TotalCalories = 0;
                mealPlanEntity.TotalProtein = 0;
                mealPlanEntity.TotalFat = 0;
                mealPlanEntity.TotalCarbohydrates = 0;
            }

            return new MealPlan
            {
                Id = mealPlanEntity.Id,
                UserId = mealPlanEntity.UserId,
                Date = mealPlanEntity.Date,
                TotalCalories = mealPlanEntity.TotalCalories,
                TotalProtein = mealPlanEntity.TotalProtein,
                TotalFat = mealPlanEntity.TotalFat,
                TotalCarbohydrates = mealPlanEntity.TotalCarbohydrates,
                MealPlanItems = mealPlanEntity.MealPlanItems.Select(item => new MealPlanItem
                {
                    Id = item.Id,
                    MealPlanId = item.MealPlanId,
                    MealTimeId = item.MealTimeId,
                    ProductId = item.ProductId,
                    RecipeId = item.RecipeId,
                    Amount = item.Amount
                }).ToList()
            };
        }



        public async Task<int> CreateMealPlanAsync(MealPlan mealPlan)
        {
            var mealPlanEntity = new MealPlanEntity
            {
                UserId = mealPlan.UserId,
                Date = mealPlan.Date,
                TotalCalories = mealPlan.TotalCalories,
                TotalProtein = mealPlan.TotalProtein,
                TotalFat = mealPlan.TotalFat,
                TotalCarbohydrates = mealPlan.TotalCarbohydrates,
                MealPlanItems = new List<MealPlanItemEntity>()
            };

            await _repository.CreateAsync(mealPlanEntity);

            // После сохранения контекста Entity Framework автоматически заполнит Id
            return mealPlanEntity.Id;
        }

        public async Task<List<MealPlan>> GetMealPlansByUserIdAndDateRangeAsync(Guid userId, DateOnly startDate, DateOnly endDate)
        {
            var mealPlanEntities = await _repository.GetMealPlansByUserIdAndDateRangeAsync(userId, startDate, endDate);

            var mealPlans = new List<MealPlan>();

            foreach (var entity in mealPlanEntities)
            {
                mealPlans.Add(new MealPlan
                {
                    Id = entity.Id,
                    UserId = entity.UserId,
                    Date = entity.Date,
                    TotalCalories = entity.TotalCalories,
                    TotalProtein = entity.TotalProtein,
                    TotalFat = entity.TotalFat,
                    TotalCarbohydrates = entity.TotalCarbohydrates,
                    MealPlanItems = entity.MealPlanItems.Select(item => new MealPlanItem
                    {
                        Id = item.Id,
                        MealPlanId = item.MealPlanId,
                        MealTimeId = item.MealTimeId,
                        ProductId = item.ProductId,
                        RecipeId = item.RecipeId,
                        Amount = item.Amount
                    }).ToList()
                });
            }

            return mealPlans;
        }
    }
}