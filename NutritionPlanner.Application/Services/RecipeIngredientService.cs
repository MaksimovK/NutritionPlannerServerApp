using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Repositories.Interfaces;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.Application.Services
{
    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly IRecipeIngredientRepository _repository;

        public RecipeIngredientService(IRecipeIngredientRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RecipeIngredient>> GetByRecipeIdAsync(int recipeId)
        {
            var ingredients = await _repository.GetByRecipeIdAsync(recipeId);
            var result = new List<RecipeIngredient>();
            foreach (var ingredient in ingredients)
            {
                result.Add(new RecipeIngredient
                {
                    Id = ingredient.Id,
                    RecipeId = ingredient.RecipeId,
                    ProductId = ingredient.ProductId,
                    Amount = ingredient.Amount
                });
            }
            return result;
        }

        public async Task<int> CreateRecipeIngredientAsync(RecipeIngredient ingredient)
        {
            var ingredientEntity = new RecipeIngredientEntity
            {
                Id = ingredient.Id,
                RecipeId = ingredient.RecipeId,
                ProductId = ingredient.ProductId,
                Amount = ingredient.Amount
            };
            await _repository.CreateAsync(ingredientEntity);
            return ingredientEntity.Id;
        }
    }
}
