using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Repositories.Interfaces;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.Application.Utilities;

namespace NutritionPlanner.Application.Services
{
    public class UserGoalService : IUserGoalService
    {
        private readonly IUserGoalRepository _repository;
        private readonly IUserService _userService;
        private readonly INutrition _nutritionService;

        public UserGoalService(IUserGoalRepository repository, IUserService userService, INutrition nutritionSerivce)
        {
            _repository = repository;
            _userService = userService;
            _nutritionService = nutritionSerivce;
        }

        public async Task<List<UserGoal>> GetByUserIdAsync(Guid userId)
        {
            var userGoals = await _repository.GetByUserIdAsyncList(userId);
            var result = new List<UserGoal>();

            foreach (var userGoal in userGoals)
            {
                result.Add(new UserGoal
                {
                    Id = userGoal.Id,
                    UserId = userGoal.UserId,
                    GoalTypeId = userGoal.GoalTypeId,
                    Calories = userGoal.Calories,
                    Protein = userGoal.Protein,
                    Fat = userGoal.Fat,
                    Carbohydrates = userGoal.Carbohydrates,
                    CreatedAt = userGoal.CreatedAt
                });
            }

            return result;
        }

        public async Task CreateUserGoalAsync(UserGoal userGoal)
        {
            var userGoalEntity = ToEntity(userGoal);
            await _repository.CreateAsync(userGoalEntity);
        }

        public async Task UpdateUserGoalAsync(Guid userId, int goalTypeId)
        {
            // Получаем одну цель по userId
            var userGoalEntity = await _repository.GetByUserIdAsync(userId);
            if (userGoalEntity == null)
            {
                throw new Exception("Цель не найдена");
            }

            // Проверяем, существует ли цель с данным goalTypeId
            var validGoalTypeIds = new List<int> { 1, 2, 3 }; // Список допустимых goalTypeId
            if (!validGoalTypeIds.Contains(goalTypeId))
            {
                throw new Exception($"Цель с ID {goalTypeId} не существует.");
            }

            // Получаем данные пользователя
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("Пользователь не найден");
            }

            // Рассчитываем калории и БЖУ на основе данных пользователя и goalTypeId
            var calories = _nutritionService.CalculateCalories(user.Weight, user.Height, user.Age, user.Gender, user.ActivityLevelId, goalTypeId);
            var bju = _nutritionService.CalculateBJU(calories, user.Weight, goalTypeId);

            // Обновляем данные цели
            userGoalEntity.GoalTypeId = goalTypeId;
            userGoalEntity.Calories = calories;
            userGoalEntity.Protein = bju.Protein;
            userGoalEntity.Fat = bju.Fat;
            userGoalEntity.Carbohydrates = bju.Carbohydrates;

            // Сохраняем изменения
            await _repository.UpdateAsync(userGoalEntity);
        }

        private UserGoalEntity ToEntity(UserGoal userGoal)
        {
            return new UserGoalEntity
            {
                Id = userGoal.Id,
                UserId = userGoal.UserId,
                GoalTypeId = userGoal.GoalTypeId,
                Calories = userGoal.Calories,
                Protein = userGoal.Protein,
                Fat = userGoal.Fat,
                Carbohydrates = userGoal.Carbohydrates,
                CreatedAt = userGoal.CreatedAt
            };
        }

        private UserGoal ToModel(UserGoalEntity userGoalEntity)
        {
            return new UserGoal
            {
                Id = userGoalEntity.Id,
                UserId = userGoalEntity.UserId,
                GoalTypeId = userGoalEntity.GoalTypeId,
                Calories = userGoalEntity.Calories,
                Protein = userGoalEntity.Protein,
                Fat = userGoalEntity.Fat,
                Carbohydrates = userGoalEntity.Carbohydrates,
                CreatedAt = userGoalEntity.CreatedAt
            };
        }
    }
}
