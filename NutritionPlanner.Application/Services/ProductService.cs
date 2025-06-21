using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Repositories.Interfaces;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        private Product MapToProduct(ProductEntity entity)
        {
            return new Product
            {
                Id = entity.Id,
                Name = entity.Name,
                Weight = entity.Weight,
                Calories = entity.Calories,
                Protein = entity.Protein,
                Fat = entity.Fat,
                Carbohydrates = entity.Carbohydrates,
                Barcode = entity.Barcode,
                IsApproved = entity.IsApproved,
                CreatedByUserId = entity.CreatedByUserId
            };
        }

        public async Task<List<Product>> GetPaginatedProductsAsync(
          int page,
          int size,
          Guid? userId,
          Role userRole,
          ProductFilter filter = null)
        {
            var entities = await _repository.GetPaginatedAsync(
                page, size, userId, userRole, filter);
            return entities.Select(MapToProduct).ToList();
        }

        public async Task<List<Product>> GetAllAsync(Guid? userId, Role userRole)
        {
            var entities = await _repository.GetAllAsync(userId, userRole);
            return entities.Select(MapToProduct).ToList();
        }

        public async Task<List<Product>> GetProductsByIdsAsync(List<int> ids, Guid? userId, Role userRole)
        {
            var entities = await _repository.GetByIdsAsync(ids, userId, userRole);
            return entities.Select(MapToProduct).ToList();
        }

        public async Task<Product> GetByIdAsync(int id, Guid? userId, Role userRole)
        {
            var entity = await _repository.GetByIdAsync(id, userId, userRole);
            if (entity == null) return null;
            return MapToProduct(entity);
        }

        public async Task<List<Product>> GetProductsByNameAsync(
         string name,
         Guid? userId,
         Role userRole,
         ProductFilter filter = null)
        {
            var entities = await _repository.GetByNameAsync(
                name, userId, userRole, filter);
            return entities.Select(MapToProduct).ToList();
        }

        public async Task UpdateProductAsync(Product product, Guid? currentUserId, Role currentUserRole)
        {
            // Получаем существующий продукт с проверкой прав доступа
            var existingEntity = await _repository.GetByIdAsync(
                product.Id,
                currentUserId,
                currentUserRole
            );

            if (existingEntity == null)
                throw new KeyNotFoundException("Product not found or access denied");

            // Проверяем права на обновление:
            // - Админ может обновлять любые продукты
            // - Пользователь может обновлять только свои продукты
            if (currentUserRole != Role.Admin && existingEntity.CreatedByUserId != currentUserId)
                throw new UnauthorizedAccessException("You can only update your own products");

            // Проверка штрихкода
            if (product.Barcode != existingEntity.Barcode)
            {
                if (!string.IsNullOrEmpty(product.Barcode))
                {
                    var existingWithBarcode = await _repository.GetByBarcodeAsync(
                        product.Barcode,
                        null,
                        Role.Admin
                    );

                    if (existingWithBarcode != null && existingWithBarcode.Id != product.Id)
                        throw new InvalidOperationException("Штрихкод уже используется другим продуктом");
                }
            }

            // Обновляем только разрешенные поля
            existingEntity.Name = product.Name;
            existingEntity.Weight = product.Weight;
            existingEntity.Calories = product.Calories;
            existingEntity.Protein = product.Protein;
            existingEntity.Fat = product.Fat;
            existingEntity.Carbohydrates = product.Carbohydrates;
            existingEntity.Barcode = product.Barcode;

            // Для администраторов сохраняем статус модерации
            if (currentUserRole == Role.Admin)
            {
                existingEntity.IsApproved = product.IsApproved;
            }

            await _repository.UpdateAsync(existingEntity);
        }

        public async Task<int> CreateProductAsync(Product product, Guid? userId, Role userRole)
        {
            if (!string.IsNullOrEmpty(product.Barcode))
            {
                var existing = await _repository.GetByBarcodeAsync(product.Barcode, null, Role.Admin);
                if (existing != null)
                    throw new InvalidOperationException("Продукт с таким штрихкодом уже существует");
            }

            var entity = new ProductEntity
            {
                Name = product.Name,
                Weight = product.Weight,
                Calories = product.Calories,
                Protein = product.Protein,
                Fat = product.Fat,
                Carbohydrates = product.Carbohydrates,
                Barcode = product.Barcode,
                IsApproved = userRole == Role.Admin,
                CreatedByUserId = userId
            };

            await _repository.CreateAsync(entity);
            return entity.Id;
        }

        public async Task DeleteProductAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<Product?> GetProductByBarcodeAsync(string barcode, Guid? userId, Role userRole)
        {
            var entity = await _repository.GetByBarcodeAsync(barcode, userId, userRole);
            if (entity == null) return null;
            return MapToProduct(entity);
        }

        public async Task ApproveProductAsync(int productId)
        {
            var entity = await _repository.GetByIdAsync(productId, null, Role.Admin);
            if (entity == null) return;

            entity.IsApproved = true;
            await _repository.UpdateAsync(entity);
        }

        public async Task<List<Product>> GetUnapprovedProductsAsync()
        {
            var entities = await _repository.GetUnapprovedAsync();
            return entities.Select(MapToProduct).ToList();
        }
    }
}