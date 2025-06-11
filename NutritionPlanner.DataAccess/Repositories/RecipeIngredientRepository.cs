using Microsoft.EntityFrameworkCore;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;

namespace NutritionPlanner.DataAccess.Repositories
{
    public class RecipeIngredientRepository : IRecipeIngredientRepository
    {
        private readonly NutritionPlannerDbContext _context;

        public RecipeIngredientRepository(NutritionPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<List<RecipeIngredientEntity>> GetByRecipeIdAsync(int recipeId)
        {
            return await _context.RecipeIngredients
                .Where(ri => ri.RecipeId == recipeId)
                .ToListAsync();
        }

        public async Task CreateAsync(RecipeIngredientEntity recipeIngredient)
        {
            await _context.RecipeIngredients.AddAsync(recipeIngredient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RecipeIngredientEntity recipeIngredient)
        {
            _context.RecipeIngredients.Update(recipeIngredient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var recipeIngredient = await _context.RecipeIngredients.FindAsync(id);
            if (recipeIngredient != null)
            {
                _context.RecipeIngredients.Remove(recipeIngredient);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateBatchAsync(IEnumerable<RecipeIngredientEntity> ingredients)
        {
            await _context.RecipeIngredients.AddRangeAsync(ingredients);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RecipeIngredientEntity>> GetByRecipeIdsAsync(IEnumerable<int> recipeIds)
        {
            return await _context.RecipeIngredients
                .Where(ri => recipeIds.Contains(ri.RecipeId))
                .ToListAsync();
        }

        public async Task<List<RecipeIngredientEntity>> GetByRecipeIdsWithProductsAsync(IEnumerable<int> recipeIds)
        {
            return await _context.RecipeIngredients
                .Include(ri => ri.Product) // Загружаем связанные продукты
                .Where(ri => recipeIds.Contains(ri.RecipeId))
                .ToListAsync();
        }
    }
}
