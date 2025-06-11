using Microsoft.EntityFrameworkCore;
using NutritionPlanner.Core.Models;
using NutritionPlanner.DataAccess.Entities;
using NutritionPlanner.DataAccess.Repositories.Interfaces;

namespace NutritionPlanner.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly NutritionPlannerDbContext _context;

        public ProductRepository(NutritionPlannerDbContext context)
        {
            _context = context;
        }

        private IQueryable<ProductEntity> ApplyFilter(IQueryable<ProductEntity> query, Guid? userId, Role userRole)
        {
            if (userRole != Role.Admin)
            {
                query = query.Where(p =>
                    p.IsApproved ||
                    (p.CreatedByUserId == userId && !p.IsApproved)
                );
            }
            return query;
        }

        public async Task<List<ProductEntity>> GetPaginatedAsync(int page, int size, Guid? userId, Role userRole)
        {
            var query = _context.Products.AsQueryable();
            query = ApplyFilter(query, userId, userRole);

            return await query
                .Skip(page * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<ProductEntity> GetByIdAsync(int id, Guid? userId, Role userRole)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            // Для администраторов возвращаем любой продукт
            if (userRole == Role.Admin) return product;

            // Для обычных пользователей:
            // - Возвращаем подтвержденные продукты
            // - Или неподтвержденные, но созданные текущим пользователем
            if (product.IsApproved || product.CreatedByUserId == userId)
                return product;

            return null;
        }

        public async Task<List<ProductEntity>> GetAllAsync(Guid? userId, Role userRole)
        {
            var query = _context.Products.AsQueryable();
            query = ApplyFilter(query, userId, userRole);
            return await query.ToListAsync();
        }

        public async Task CreateAsync(ProductEntity product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductEntity product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ProductEntity>> GetByIdsAsync(List<int> ids, Guid? userId, Role userRole)
        {
            var query = _context.Products
                .Where(p => ids.Contains(p.Id))
                .AsQueryable();

            query = ApplyFilter(query, userId, userRole);
            return await query.ToListAsync();
        }

        public async Task<List<ProductEntity>> GetByNameAsync(string name, Guid? userId, Role userRole)
        {
            var query = _context.Products
                .Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{name.ToLower()}%"))
                .AsQueryable();

            query = ApplyFilter(query, userId, userRole);
            return await query.ToListAsync();
        }

        public async Task<ProductEntity?> GetByBarcodeAsync(string barcode, Guid? userId, Role userRole)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Barcode == barcode);

            if (product == null) return null;
            if (userRole == Role.Admin) return product;
            if (product.IsApproved) return product;
            if (product.CreatedByUserId == userId) return product;

            return null;
        }

        public async Task<List<ProductEntity>> GetUnapprovedAsync()
        {
            return await _context.Products
                .Where(p => !p.IsApproved)
                .ToListAsync();
        }
    }
}