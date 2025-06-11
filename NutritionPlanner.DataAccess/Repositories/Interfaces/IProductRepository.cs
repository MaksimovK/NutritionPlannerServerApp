using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Entities;

namespace NutritionPlanner.DataAccess.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductEntity>> GetPaginatedAsync(int page, int size, Guid? userId, Role userRole);
        Task<ProductEntity> GetByIdAsync(int id, Guid? userId, Role userRole);
        Task<List<ProductEntity>> GetAllAsync(Guid? userId, Role userRole);
        Task CreateAsync(ProductEntity product);
        Task UpdateAsync(ProductEntity product);
        Task DeleteAsync(int id);
        Task<List<ProductEntity>> GetByIdsAsync(List<int> ids, Guid? userId, Role userRole);
        Task<List<ProductEntity>> GetByNameAsync(string name, Guid? userId, Role userRole);
        Task<ProductEntity?> GetByBarcodeAsync(string barcode, Guid? userId, Role userRole);
        Task<List<ProductEntity>> GetUnapprovedAsync();
    }
}