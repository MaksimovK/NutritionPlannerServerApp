using NutritionPlanner.Core.Models;

namespace NutritionPlanner.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetPaginatedProductsAsync(int page, int size, Guid? userId, Role userRole, ProductFilter filter = null);
        Task<List<Product>> GetAllAsync(Guid? userId, Role userRole);
        Task<List<Product>> GetProductsByIdsAsync(List<int> ids, Guid? userId, Role userRole);
        Task<Product> GetByIdAsync(int id, Guid? userId, Role userRole);
        Task<List<Product>> GetProductsByNameAsync(string name, Guid? userId, Role userRole, ProductFilter filter = null);
        Task UpdateProductAsync(Product product, Guid? currentUserId, Role currentUserRole);
        Task<int> CreateProductAsync(Product product, Guid? userId, Role userRole);
        Task DeleteProductAsync(int id);
        Task<Product?> GetProductByBarcodeAsync(string barcode, Guid? userId, Role userRole);
        Task ApproveProductAsync(int productId);
        Task<List<Product>> GetUnapprovedProductsAsync();
    }
}