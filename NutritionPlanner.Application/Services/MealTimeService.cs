using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Repositories.Interfaces;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.Application.Services
{
    public class MealTimeService : IMealTimeService
    {
        private readonly IMealTimeRepository _repository;

        public MealTimeService(IMealTimeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MealTime>> GetAllAsync()
        {
            var mealTimes = await _repository.GetAllAsync();
            var result = new List<MealTime>();
            foreach (var mealTime in mealTimes)
            {
                result.Add(new MealTime
                {
                    Id = mealTime.Id,
                    Name = mealTime.Name,
                    Description = mealTime.Description
                });
            }
            return result;
        }

        public async Task<int> CreateMealTimeAsync(MealTime mealTime)
        {
            var entity = new MealTimeEntity
            {
                Name = mealTime.Name,
                Description = mealTime.Description
            };
            await _repository.CreateAsync(entity);
            return entity.Id;
        }

        public async Task UpdateMealTimeAsync(MealTime mealTime)
        {
            var entity = new MealTimeEntity
            {
                Id = mealTime.Id,
                Name = mealTime.Name,
                Description = mealTime.Description
            };
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteMealTimeAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
