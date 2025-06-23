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
            var exists = await _repository.ExistsAsync(progress.UserId, progress.Date);

            if (exists)
            {
                var existing = await _repository.GetProgressByUserIdAndDateAsync(progress.UserId, progress.Date);

                // Обновляем существующую запись
                existing.Weight = progress.Weight;
                existing.CaloriesConsumed = progress.CaloriesConsumed;
                existing.ProteinConsumed = progress.ProteinConsumed;
                existing.FatConsumed = progress.FatConsumed;
                existing.CarbohydratesConsumed = progress.CarbohydratesConsumed;
                existing.WaterConsumed = progress.WaterConsumed;
                existing.ActivityMinutes = progress.ActivityMinutes;

                await _repository.UpdateAsync(existing);
                return existing.Id;
            }
            else
            {
                // Создаем новую запись
                var progressEntity = new UserProgressEntity
                {
                    UserId = progress.UserId,
                    Date = progress.Date,
                    Weight = progress.Weight,
                    CaloriesConsumed = progress.CaloriesConsumed,
                    ProteinConsumed = progress.ProteinConsumed,
                    FatConsumed = progress.FatConsumed,
                    CarbohydratesConsumed = progress.CarbohydratesConsumed,
                    WaterConsumed = progress.WaterConsumed,
                    ActivityMinutes = progress.ActivityMinutes
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
