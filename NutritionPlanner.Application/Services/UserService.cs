using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Repositories.Interfaces;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.Application.Utilities;

namespace NutritionPlanner.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _userRepository;
        private readonly IUserGoalRepository _userGoalRepository;
        private readonly INutrition _nutritionService;

        public UserService(IUsersRepository userRepository, IUserGoalRepository userGoalRepository, INutrition nutritionService)
        {
            _userRepository = userRepository;
            _userGoalRepository = userGoalRepository;
            _nutritionService = nutritionService;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var userEntity = await _userRepository.GetByIdAsync(userId);
            if (userEntity == null) return null;

            return new User
            {
                Id = userEntity.Id,
                Name = userEntity.Name,
                Email = userEntity.Email,
                Age = userEntity.Age,
                Gender = userEntity.Gender,
                Height = userEntity.Height,
                Weight = userEntity.Weight,
                ActivityLevelId = userEntity.ActivityLevelId,
                PasswordHash = userEntity.PasswordHash,
                CreatedAt = userEntity.CreatedAt,
                Role = userEntity.Role
            };
        }

        public async Task<Guid> CreateUserAsync(User user)
        {

            var userEntity = new UserEntity
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Age = user.Age,
                Gender = user.Gender,
                Height = user.Height,
                Weight = user.Weight,
                ActivityLevelId = user.ActivityLevelId,
                PasswordHash = user.PasswordHash,
                CreatedAt = user.CreatedAt,
                Role = user.Role
            };
            await _userRepository.CreateAsync(userEntity);
            return userEntity.Id;
        }

        public async Task UpdateUserAsync(User user)
        {
            var userEntity = await _userRepository.GetByIdAsync(user.Id);
            if (userEntity == null)
                throw new KeyNotFoundException($"User with ID {user.Id} not found");

            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null && existingUser.Id != user.Id)
                throw new InvalidOperationException("Email занят");

            bool isDataChanged =
                userEntity.ActivityLevelId != user.ActivityLevelId ||
                userEntity.Weight != user.Weight ||
                userEntity.Height != user.Height ||
                userEntity.Age != user.Age;


            userEntity.Name = user.Name;
            userEntity.Email = user.Email;
            userEntity.Age = user.Age;
            userEntity.Gender = user.Gender;
            userEntity.Height = user.Height;
            userEntity.Weight = user.Weight;
            userEntity.ActivityLevelId = user.ActivityLevelId;
            userEntity.PasswordHash = user.PasswordHash;
            userEntity.Role = user.Role;

            await _userRepository.UpdateAsync(userEntity);

            if (isDataChanged)
            {
                var userGoals = await _userGoalRepository.GetByUserIdAsyncList(user.Id);

                foreach (var goalEntity in userGoals)
                {
                    var calories = _nutritionService.CalculateCalories(user.Weight, user.Height, user.Age, user.Gender, user.ActivityLevelId, goalEntity.GoalTypeId);
                    var bju = _nutritionService.CalculateBJU(calories, user.Weight, goalEntity.GoalTypeId);

                    goalEntity.Calories = calories;
                    goalEntity.Protein = bju.Protein;
                    goalEntity.Fat = bju.Fat;
                    goalEntity.Carbohydrates = bju.Carbohydrates;

                    await _userGoalRepository.UpdateAsync(goalEntity);
                }
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new User
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            });
        }

        public async Task UpdateUserRoleAsync(Guid userId, Role newRole)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");

            user.Role = newRole;
            await _userRepository.UpdateAsync(user);
        }
    }
}
