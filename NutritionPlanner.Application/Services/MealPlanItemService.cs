using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Repositories.Interfaces;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.Application.Services
{
    public class MealPlanItemService : IMealPlanItemService
    {
        private readonly IMealPlanItemRepository _repository;

        public MealPlanItemService(IMealPlanItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MealPlanItem>> GetByMealPlanIdAsync(int mealPlanId)
        {
            var mealPlanItems = await _repository.GetByMealPlanIdAsync(mealPlanId);
            return mealPlanItems.Select(item => new MealPlanItem
            {
                Id = item.Id,
                MealPlanId = item.MealPlanId,
                MealTimeId = item.MealTimeId,
                ProductId = item.ProductId,
                RecipeId = item.RecipeId,
                Amount = item.Amount
            }).ToList();
        }

        public async Task DeleteMealPlanItemAsync(int id)
        {
            var mealPlanItem = await _repository.GetByIdAsync(id);
            if (mealPlanItem == null)
            {
                throw new ArgumentException($"Meal plan item with ID {id} not found.");
            }

            var mealPlan = await _repository.GetMealPlanByIdAsync(mealPlanItem.MealPlanId);
            if (mealPlan != null)
            {
                // Обновляем кбжу
                var product = await _repository.GetProductByIdAsync(mealPlanItem.ProductId.Value);
                if (product != null)
                {
                        
                    mealPlan.TotalCalories -= product.Calories * mealPlanItem.Amount;
                    mealPlan.TotalProtein -= product.Protein * mealPlanItem.Amount;
                    mealPlan.TotalFat -= product.Fat * mealPlanItem.Amount;
                    mealPlan.TotalCarbohydrates -= product.Carbohydrates * mealPlanItem.Amount;
                }

                await _repository.UpdateMealPlanAsync(mealPlan);
            }

            await _repository.DeleteAsync(mealPlanItem.Id);
        }

        public async Task<int> AddMealPlanItemAsync(MealPlanItem mealPlanItem)
        {
            // Проверка существования MealPlan
            var mealPlan = await _repository.GetMealPlanByIdAsync(mealPlanItem.MealPlanId);
            if (mealPlan == null)
            {
                throw new ArgumentException($"MealPlan с ID {mealPlanItem.MealPlanId} не найден.");
            }

            // Проверяем, что указан либо продукт, либо рецепт
            if (!mealPlanItem.ProductId.HasValue && !mealPlanItem.RecipeId.HasValue)
            {
                throw new ArgumentException("Должен быть указан либо продукт, либо рецепт.");
            }

            // Поиск существующего элемента
            MealPlanItemEntity existingItem = null;

            if (mealPlanItem.ProductId.HasValue)
            {
                // Для продуктов: ищем по ProductId и MealTimeId
                existingItem = mealPlan.MealPlanItems
                    .FirstOrDefault(item =>
                        item.ProductId == mealPlanItem.ProductId &&
                        item.MealTimeId == mealPlanItem.MealTimeId);
            }
            else if (mealPlanItem.RecipeId.HasValue)
            {
                // Для рецептов: ищем по RecipeId и MealTimeId
                existingItem = mealPlan.MealPlanItems
                    .FirstOrDefault(item =>
                        item.RecipeId == mealPlanItem.RecipeId &&
                        item.MealTimeId == mealPlanItem.MealTimeId);
            }

            if (existingItem != null)
            {
                // Если элемент уже существует, обновляем количество
                existingItem.Amount = mealPlanItem.Amount;
                await _repository.UpdateAsync(existingItem);
            }
            else
            {
                // Создание нового элемента
                var mealPlanItemEntity = new MealPlanItemEntity
                {
                    MealPlanId = mealPlanItem.MealPlanId,
                    MealTimeId = mealPlanItem.MealTimeId,
                    ProductId = mealPlanItem.ProductId,
                    RecipeId = mealPlanItem.RecipeId,
                    Amount = mealPlanItem.Amount
                };

                // Добавление нового элемента в MealPlan
                mealPlan.MealPlanItems.Add(mealPlanItemEntity);
                await _repository.CreateAsync(mealPlanItemEntity);
            }

            // Перерасчёт кБЖУ для MealPlan
            await RecalculateMealPlanNutrition(mealPlan.Id);

            // Возвращаем ID существующего или нового элемента
            return existingItem?.Id ?? mealPlan.MealPlanItems.Last().Id;
        }

        private async Task RecalculateMealPlanNutrition(int mealPlanId)
        {
            var mealPlan = await _repository.GetMealPlanByIdAsync(mealPlanId);
            if (mealPlan == null) return;

            // Обнуляем кБЖУ перед перерасчётом
            mealPlan.TotalCalories = 0;
            mealPlan.TotalProtein = 0;
            mealPlan.TotalFat = 0;
            mealPlan.TotalCarbohydrates = 0;

            // Перерасчёт кБЖУ на основе всех MealPlanItems
            foreach (var item in mealPlan.MealPlanItems)
            {
                if (item.ProductId.HasValue)
                {
                    var product = await _repository.GetProductByIdAsync(item.ProductId.Value);
                    if (product != null)
                    {
                        mealPlan.TotalCalories += product.Calories * item.Amount;
                        mealPlan.TotalProtein += product.Protein * item.Amount;
                        mealPlan.TotalFat += product.Fat * item.Amount;
                        mealPlan.TotalCarbohydrates += product.Carbohydrates * item.Amount;
                    }
                }
                else if (item.RecipeId.HasValue)
                {
                    var recipe = await _repository.GetRecipeByIdAsync(item.RecipeId.Value);
                    if (recipe != null)
                    {
                        foreach (var ingredient in recipe.Ingredients)
                        {
                            var product = await _repository.GetProductByIdAsync(ingredient.ProductId);
                            if (product != null)
                            {
                                var amountRatio = item.Amount / recipe.TotalWeight;
                                var ingredientAmount = ingredient.Amount * amountRatio;

                                mealPlan.TotalCalories += Math.Round(product.Calories * ingredientAmount / 100m, 2);
                                mealPlan.TotalProtein += Math.Round(product.Protein * ingredientAmount / 100m, 2);
                                mealPlan.TotalFat += Math.Round(product.Fat * ingredientAmount / 100m, 2);
                                mealPlan.TotalCarbohydrates += Math.Round(product.Carbohydrates * ingredientAmount / 100m, 2);
                            }
                        }
                    }
                }
            }

            // Обновление MealPlan в базе данных
            await _repository.UpdateMealPlanAsync(mealPlan);
        }

        public async Task UpdateMealPlanItemAsync(MealPlanItem mealPlanItem)
        {
            // Получаем MealPlanItem из репозитория
            var mealPlanItemEntity = await _repository.GetByIdAsync(mealPlanItem.Id);
            if (mealPlanItemEntity == null)
            {
                throw new ArgumentException("Meal plan item not found");
            }

            // Сохраняем старые данные для корректного перерасчёта кБЖУ
            var oldProductId = mealPlanItemEntity.ProductId;
            var oldAmount = mealPlanItemEntity.Amount;

            // Обновляем поля MealPlanItem
            mealPlanItemEntity.ProductId = mealPlanItem.ProductId;
            mealPlanItemEntity.RecipeId = mealPlanItem.RecipeId;
            mealPlanItemEntity.Amount = mealPlanItem.Amount;

            await _repository.UpdateAsync(mealPlanItemEntity);

            // Перерасчёт кБЖУ для MealPlan
            var mealPlan = await _repository.GetMealPlanByIdAsync(mealPlanItem.MealPlanId);

            if (mealPlan == null)
            {
                throw new ArgumentException("Meal plan not found");
            }

            // Обнуляем кБЖУ перед перерасчётом
            mealPlan.TotalCalories = 0;
            mealPlan.TotalProtein = 0;
            mealPlan.TotalFat = 0;
            mealPlan.TotalCarbohydrates = 0;

            // Перебираем все MealPlanItems для данного MealPlan
            foreach (var item in mealPlan.MealPlanItems)
            {
                var product = await _repository.GetProductByIdAsync(item.ProductId.Value);
                if (product != null)
                {
                    var multiplier = item.Amount;
                    mealPlan.TotalCalories += product.Calories * multiplier;
                    mealPlan.TotalProtein += product.Protein * multiplier;
                    mealPlan.TotalFat += product.Fat * multiplier;
                    mealPlan.TotalCarbohydrates += product.Carbohydrates * multiplier;
                }
            }

            // Обновляем MealPlan в базе данных
            await _repository.UpdateMealPlanAsync(mealPlan);
        }
    }
}