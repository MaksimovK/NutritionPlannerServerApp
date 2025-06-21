using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.Core.DTO.NutritionPlanner.Core.Models;
using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;

namespace NutritionPlanner.Application.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IRecipeIngredientRepository _ingredientRepository;
        private readonly ICurrentUserService _currentUserService;

        public RecipeService(
            IRecipeRepository recipeRepository,
            IRecipeIngredientRepository ingredientRepository,
            ICurrentUserService currentUserService)
        {
            _recipeRepository = recipeRepository;
            _ingredientRepository = ingredientRepository;
            _currentUserService = currentUserService;
        }

        public async Task<List<RecipeWithNutritionDto>> GetAllAsync(RecipeFilter filter)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            var role = currentUser?.Role ?? Role.User;
            var userId = currentUser?.Id;

            // Загружаем все рецепты
            var all = await _recipeRepository.GetAllAsync();

            // Фильтрация: обычные видят только одобренные или свои собственные
            var filtered = all.Where(r =>
                r.IsApproved
                || (role == Role.Admin)
                || (r.CreatedByUserId == userId)
            ).ToList();

            var result = await MapWithNutritionAsync(filtered);
            return ApplyRecipeFilter(result, filter);
        }

        public async Task<RecipeWithNutritionDto> GetByIdAsync(int id)
        {
            var entity = await _recipeRepository.GetByIdAsync(id);
            if (entity == null) return null;

            var currentUser = _currentUserService.GetCurrentUser();
            var role = currentUser?.Role ?? Role.User;
            var userId = currentUser?.Id;

            // Доступ только к одобренному или собственному рецепту или админу
            if (!entity.IsApproved
                && role != Role.Admin
                && entity.CreatedByUserId != userId)
            {
                return null;
            }

            var list = await MapWithNutritionAsync(new List<RecipeEntity> { entity });
            return list.FirstOrDefault();
        }

        public async Task<RecipeWithNutritionDto> CreateRecipeAsync(RecipeDto dto)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser == null)
                throw new UnauthorizedAccessException("Необходимо пройти аутентификацию");

            // Создаём рецепт
            var entity = new RecipeEntity
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedByUserId = currentUser.Id,
                // Админ может сразу одобрить, иначе false
                IsApproved = currentUser.Role == Role.Admin
            };

            await _recipeRepository.CreateAsync(entity);

            // Создаём ингредиенты
            var ingredients = dto.Ingredients.Select(i => new RecipeIngredientEntity
            {
                RecipeId = entity.Id,
                ProductId = i.ProductId,
                Amount = i.Amount
            }).ToList();

            await _ingredientRepository.CreateBatchAsync(ingredients);

            // Вернём DTO
            return await GetByIdAsync(entity.Id);
        }

        public async Task<List<RecipeWithNutritionDto>> SearchByNameAsync(string name, RecipeFilter filter)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            var role = currentUser?.Role ?? Role.User;
            var userId = currentUser?.Id;

            var found = await _recipeRepository.SearchByNameAsync(name);
            var filtered = found.Where(r =>
                r.IsApproved
                || role == Role.Admin
                || r.CreatedByUserId == userId
            ).ToList();

            var result = await MapWithNutritionAsync(filtered);
            return ApplyRecipeFilter(result, filter);
        }

        public async Task<List<RecipeWithNutritionDto>> GetUnapprovedRecipesAsync()
        {
            // Только для админа
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser?.Role != Role.Admin)
                throw new UnauthorizedAccessException("Доступ запрещён");

            var list = await _recipeRepository.GetUnapprovedAsync();
            return await MapWithNutritionAsync(list);
        }

        public async Task ApproveRecipeAsync(int id)
        {
            // Только для админа
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser?.Role != Role.Admin)
                throw new UnauthorizedAccessException("Доступ запрещён");

            await _recipeRepository.ApproveAsync(id);
        }
        public async Task DeleteRecipeAsync(int id)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser?.Role != Role.Admin)
                throw new UnauthorizedAccessException("Доступ запрещён");

            await _recipeRepository.DeleteAsync(id);
        }

        public async Task<List<RecipeWithNutritionDto>> GetByIdsAsync(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return new List<RecipeWithNutritionDto>();

            var entities = await _recipeRepository.GetByIdsAsync(ids);

            var currentUser = _currentUserService.GetCurrentUser();
            var role = currentUser?.Role ?? Role.User;
            var userId = currentUser?.Id;

            var filtered = entities.Where(r =>
                r.IsApproved
                || (role == Role.Admin)
                || (r.CreatedByUserId == userId)
            ).ToList();

            return await MapWithNutritionAsync(filtered);
        }

        // Вспомогательный метод для расчёта КБЖУ и маппинга
        private async Task<List<RecipeWithNutritionDto>> MapWithNutritionAsync(IEnumerable<RecipeEntity> recipes)
        {
            var recipeList = recipes.ToList();
            var ids = recipeList.Select(r => r.Id).ToList();
            var allIngredients = await _ingredientRepository.GetByRecipeIdsWithProductsAsync(ids);

            var grouped = allIngredients
                .GroupBy(i => i.RecipeId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var result = new List<RecipeWithNutritionDto>();
            foreach (var r in recipeList)
            {
                var ing = grouped.ContainsKey(r.Id) ? grouped[r.Id] : new List<RecipeIngredientEntity>();
                var dtos = ing.Select(i => new RecipeIngredientDto
                {
                    Id = i.Id,
                    RecipeId = i.RecipeId,
                    ProductId = i.ProductId,
                    Amount = i.Amount,
                    Product = new ProductDto
                    {
                        Id = i.Product.Id,
                        Name = i.Product.Name,
                        Calories = i.Product.Calories,
                        Protein = i.Product.Protein,
                        Fat = i.Product.Fat,
                        Carbohydrates = i.Product.Carbohydrates
                    }
                }).ToList();

                var totalWeight = dtos.Sum(x => x.Amount);
                decimal totCal = 0, totProt = 0, totFat = 0, totCarb = 0;
                foreach (var ingDto in dtos)
                {
                    var factor = ingDto.Amount / 100m;
                    totCal += ingDto.Product.Calories * factor;
                    totProt += ingDto.Product.Protein * factor;
                    totFat += ingDto.Product.Fat * factor;
                    totCarb += ingDto.Product.Carbohydrates * factor;
                }
                var norm = totalWeight > 0 ? 100m / totalWeight : 0;

                result.Add(new RecipeWithNutritionDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    TotalWeight = totalWeight,
                    CaloriesPer100g = Math.Round(totCal * norm, 2),
                    ProteinPer100g = Math.Round(totProt * norm, 2),
                    FatPer100g = Math.Round(totFat * norm, 2),
                    CarbohydratesPer100g = Math.Round(totCarb * norm, 2),
                    Ingredients = dtos,
                    IsApproved = r.IsApproved,
                    CreatedByUserId = r.CreatedByUserId
                });
            }

            return result;
        }
        private List<RecipeWithNutritionDto> ApplyRecipeFilter(
        List<RecipeWithNutritionDto> recipes,
        RecipeFilter filter)
        {
            if (filter == null) return recipes;

            var query = recipes.AsQueryable();

            // Применяем фильтры только если они true
            if (filter.HighProtein == true)
                query = query.Where(r => r.ProteinPer100g >= 20);

            if (filter.LowCalorie == true)
                query = query.Where(r => r.CaloriesPer100g <= 100);

            if (filter.HighCalorie == true)
                query = query.Where(r => r.CaloriesPer100g >= 300);

            if (filter.LowCarb == true)
                query = query.Where(r => r.CarbohydratesPer100g <= 10);

            if (filter.HighCarb == true)
                query = query.Where(r => r.CarbohydratesPer100g >= 50);

            if (filter.LowFat == true)
                query = query.Where(r => r.FatPer100g <= 5);

            if (filter.HighFat == true)
                query = query.Where(r => r.FatPer100g >= 20);

            return query.ToList();
        }
    }
}
