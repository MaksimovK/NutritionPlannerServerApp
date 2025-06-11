using Microsoft.EntityFrameworkCore;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;

namespace NutritionPlanner.DataAccess.Repositories
{
    public class MealPlanItemRepository : IMealPlanItemRepository
    {
        private readonly NutritionPlannerDbContext _context;

        public MealPlanItemRepository(NutritionPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<List<MealPlanItemEntity>> GetByMealPlanIdAsync(int mealPlanId)
        {
            return await _context.MealPlanItems
                .Where(mpi => mpi.MealPlanId == mealPlanId)
                .ToListAsync();
        }

        public async Task CreateAsync(MealPlanItemEntity mealPlanItemEntity)
        {
            await _context.Set<MealPlanItemEntity>().AddAsync(mealPlanItemEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MealPlanItemEntity mealPlanItem)
        {
            _context.MealPlanItems.Update(mealPlanItem);
            await _context.SaveChangesAsync();
        }

        public async Task<MealPlanEntity> GetMealPlanByIdAsync(int mealPlanId)
        {
            return await _context.MealPlans
                .Include(mp => mp.MealPlanItems) // Включаем связанные элементы
                .FirstOrDefaultAsync(mp => mp.Id == mealPlanId);
        }


        public async Task<ProductEntity> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task UpdateMealPlanAsync(MealPlanEntity mealPlan)
        {
            _context.MealPlans.Update(mealPlan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var mealPlanItem = await _context.MealPlanItems.FindAsync(id);
            if (mealPlanItem != null)
            {
                _context.MealPlanItems.Remove(mealPlanItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<MealPlanItemEntity> GetByIdAsync(int id)
        {
            return await _context.MealPlanItems.FirstOrDefaultAsync(mpi => mpi.Id == id);
        }

        public async Task<RecipeEntity> GetRecipeByIdAsync(int recipeId)
        {
            return await _context.Recipes
                .Include(r => r.Ingredients)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(r => r.Id == recipeId);
        }
    }
}
