using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Repositories.Interfaces;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.Application.Services
{
    public class GoalTypeService : IGoalTypeService
    {
        private readonly IGoalTypeRepository _repository;

        public GoalTypeService(IGoalTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GoalType>> GetAllAsync()
        {
            var goalTypes = await _repository.GetAllAsync();
            return goalTypes.Select(goalType => new GoalType
            {
                Id = goalType.Id,
                Name = goalType.Name,
                Description = goalType.Description
            }).ToList();
        }

        public async Task<int> CreateGoalTypeAsync(GoalType goalType)
        {
            var goalTypeEntity = new GoalTypeEntity
            {
                Name = goalType.Name,
                Description = goalType.Description
            };
            await _repository.CreateAsync(goalTypeEntity);
            return goalTypeEntity.Id;
        }

        public async Task DeleteGoalTypeAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task UpdateGoalTypeAsync(GoalType goalType)
        {
            var entity = new GoalTypeEntity
            {
                Id = goalType.Id,
                Name = goalType.Name,
                Description = goalType.Description
            };
            await _repository.UpdateAsync(entity);
        }


    }
}
