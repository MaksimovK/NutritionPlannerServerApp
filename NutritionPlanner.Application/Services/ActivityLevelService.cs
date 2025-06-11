using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;

namespace NutritionPlanner.Application.Services
{
    public class ActivityLevelService : IActivityLevelService
    {
        private readonly IActivityLevelRepository _repository;

        public ActivityLevelService(IActivityLevelRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ActivityLevel>> GetAllAsync()
        {
            var activityLevels = await _repository.GetAllAsync();
            var result = new List<ActivityLevel>();
            foreach (var level in activityLevels)
            {
                result.Add(new ActivityLevel
                {
                    Id = level.Id,
                    Name = level.Name,
                    Description = level.Description,
                    Coefficient = level.Coefficient
                });
            }
            return result;
        }

        public async Task UpdateActivityLevelAsync(ActivityLevel activityLevel)
        {
            var entity = new ActivityLevelEntity
            {
                Id = activityLevel.Id,
                Name = activityLevel.Name,
                Description = activityLevel.Description,
                Coefficient = activityLevel.Coefficient

            };
            await _repository.UpdateAsync(entity);
        }


        public async Task<ActivityLevel> GetByIdAsync(int id)
        {
            var level = await _repository.GetByIdAsync(id);
            return new ActivityLevel
            {
                Id = level.Id,
                Name = level.Name,
                Description = level.Description
            };
        }

        public async Task<int> CreateActivityLevelAsync(ActivityLevel activityLevel)
        {
            var activityLevelEntity = new ActivityLevelEntity
            {
                Id = activityLevel.Id,
                Name = activityLevel.Name,
                Description = activityLevel.Description,
                Coefficient = activityLevel.Coefficient
            };
            await _repository.CreateAsync(activityLevelEntity);
            return activityLevelEntity.Id;
        }

        public async Task DeleteActivityLevelAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
