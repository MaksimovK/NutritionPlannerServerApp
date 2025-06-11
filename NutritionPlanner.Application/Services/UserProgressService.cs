using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Repositories.Interfaces;
using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.Application.Services
{
    public class UserProgressService : IUserProgressService
    {
        private readonly IUserProgressRepository _repository;

        public UserProgressService(IUserProgressRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<UserProgress>> GetProgressByUserIdAsync(Guid userId)
        {
            var userProgressList = await _repository.GetProgressByUserIdAsync(userId);
            var result = new List<UserProgress>();
            foreach (var progress in userProgressList)
            {
                result.Add(new UserProgress
                {
                    Id = progress.Id,
                    UserId = progress.UserId,
                    Date = progress.Date,
                    Weight = progress.Weight,
                    CaloriesConsumed = progress.CaloriesConsumed,
                    ProteinConsumed = progress.ProteinConsumed,
                    FatConsumed = progress.FatConsumed,
                    CarbohydratesConsumed = progress.CarbohydratesConsumed,
                    WaterConsumed = progress.WaterConsumed,
                    ActivityMinutes = progress.ActivityMinutes
                });
            }
            return result;
        }

        public async Task<int> AddUserProgressAsync(UserProgress progress)
        {
            // Пытаемся найти существующий прогресс для этого пользователя на эту дату
            var existingProgress = await _repository.GetProgressByUserIdAndDateAsync(progress.UserId, progress.Date);
            if (existingProgress != null)
            {
                // Если прогресс есть, обновляем его
                existingProgress.CaloriesConsumed = progress.CaloriesConsumed;
                existingProgress.ProteinConsumed = progress.ProteinConsumed;
                existingProgress.FatConsumed = progress.FatConsumed;
                existingProgress.CarbohydratesConsumed = progress.CarbohydratesConsumed;
                // Здесь можно обновить и другие параметры (вода, активность)

                await _repository.UpdateAsync(existingProgress);
                return existingProgress.Id;
            }
            else
            {
                // Если прогресса нет, создаем новый
                var progressEntity = new UserProgressEntity
                {
                    UserId = progress.UserId,
                    Date = progress.Date,
                    CaloriesConsumed = progress.CaloriesConsumed,
                    ProteinConsumed = progress.ProteinConsumed,
                    FatConsumed = progress.FatConsumed,
                    CarbohydratesConsumed = progress.CarbohydratesConsumed,
                    // Добавь другие поля
                };

                return await _repository.CreateAsync(progressEntity);
            }
        }

        public async Task<UserProgress> GetProgressByUserIdAndDateAsync(Guid userId, DateOnly date)
        {
            var progressEntity = await _repository.GetProgressByUserIdAndDateAsync(userId, date);
            if (progressEntity == null) return null;

            return new UserProgress
            {
                Id = progressEntity.Id,
                UserId = progressEntity.UserId,
                Date = progressEntity.Date,
                Weight = progressEntity.Weight,
                CaloriesConsumed = progressEntity.CaloriesConsumed,
                ProteinConsumed = progressEntity.ProteinConsumed,
                FatConsumed = progressEntity.FatConsumed,
                CarbohydratesConsumed = progressEntity.CarbohydratesConsumed,
                WaterConsumed = progressEntity.WaterConsumed,
                ActivityMinutes = progressEntity.ActivityMinutes
            };
        }

    }
}
