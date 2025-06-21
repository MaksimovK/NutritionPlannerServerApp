using Microsoft.EntityFrameworkCore;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;

namespace NutritionPlanner.DataAccess.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly NutritionPlannerDbContext _context;

        public RecipeRepository(NutritionPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<RecipeEntity> GetByIdAsync(int id)
        {
            return await _context.Recipes
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<RecipeEntity>> GetAllAsync()
        {
            return await _context.Recipes.ToListAsync();
        }

        public async Task CreateAsync(RecipeEntity recipe)
        {
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RecipeEntity recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var recipe = await GetByIdAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<RecipeEntity>> SearchByNameAsync(string name)
        {
            return await _context.Recipes
                .Where(r => r.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<List<RecipeEntity>> GetUnapprovedAsync()
        {
            return await _context.Recipes
                .Where(r => !r.IsApproved)
                .ToListAsync();
        }

        public async Task ApproveAsync(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null) return;
            recipe.IsApproved = true;
            await _context.SaveChangesAsync();
        }

        public async Task<List<RecipeEntity>> GetByIdsAsync(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return new List<RecipeEntity>();

            return await _context.Recipes
                .Where(r => ids.Contains(r.Id))
                .ToListAsync();
        }
    }
}
