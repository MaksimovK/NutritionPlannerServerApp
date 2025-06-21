using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionPlanner.Application.Services.Interfaces;
using NutritionPlanner.Core.Models;

namespace NutritionPlanner.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICurrentUserService _currentUserService;

        public ProductsController(
           IProductService productService,
           ICurrentUserService currentUserService)
        {
            _productService = productService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts(
             int page = 0,
             int size = 10,
             [FromQuery] bool? highProtein = null,
             [FromQuery] bool? lowCalorie = null,
             [FromQuery] bool? highCalorie = null,
             [FromQuery] bool? lowCarb = null,
             [FromQuery] bool? highCarb = null,
             [FromQuery] bool? lowFat = null,
             [FromQuery] bool? highFat = null)
        {
            var currentUser = _currentUserService.GetCurrentUser();

            var filter = new ProductFilter
            {
                HighProtein = highProtein,
                LowCalorie = lowCalorie,
                HighCalorie = highCalorie,
                LowCarb = lowCarb,
                HighCarb = highCarb,
                LowFat = lowFat,
                HighFat = highFat
            };

            var products = await _productService.GetPaginatedProductsAsync(
                 page, size,
                 currentUser?.Id,
                 currentUser?.Role ?? Role.User,
                 filter 
            );

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            var product = await _productService.GetByIdAsync(
                id,
                currentUser?.Id,
                currentUser?.Role ?? Role.User
            );

            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost("by-ids")]
        public async Task<ActionResult<List<Product>>> GetProductsByIds([FromBody] List<int> ids)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            var products = await _productService.GetProductsByIdsAsync(
                ids,
                currentUser?.Id,
                currentUser?.Role ?? Role.User
            );
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateProduct(Product product)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser == null) return Unauthorized();

            var productId = await _productService.CreateProductAsync(
                product,
                currentUser.Id,
                currentUser.Role
            );
            return CreatedAtAction(nameof(GetProductById), new { id = productId }, productId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Product>>> GetProductsByName(
             [FromQuery] string? name,
             [FromQuery] bool? highProtein = null,
             [FromQuery] bool? lowCalorie = null,
             [FromQuery] bool? highCalorie = null,
             [FromQuery] bool? lowCarb = null,
             [FromQuery] bool? highCarb = null,
             [FromQuery] bool? lowFat = null,
             [FromQuery] bool? highFat = null)
        {
            var currentUser = _currentUserService.GetCurrentUser();

            // Создаем фильтр из параметров запроса
            var filter = new ProductFilter
            {
                HighProtein = highProtein,
                LowCalorie = lowCalorie,
                HighCalorie = highCalorie,
                LowCarb = lowCarb,
                HighCarb = highCarb,
                LowFat = lowFat,
                HighFat = highFat
            };

            if (string.IsNullOrWhiteSpace(name))
            {
                var products = await _productService.GetPaginatedProductsAsync(
                    0, 5,
                    currentUser?.Id,
                    currentUser?.Role ?? Role.User,
                    filter // Передаем фильтр
                );
                return Ok(products);
            }

            var matched = await _productService.GetProductsByNameAsync(
                name.Trim(),
                currentUser?.Id,
                currentUser?.Role ?? Role.User,
                filter // Передаем фильтр
            );
            return Ok(matched);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
                return BadRequest("Product ID mismatch");

            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser == null)
                return Unauthorized();

            try
            {
                await _productService.UpdateProductAsync(
                    product,
                    currentUser.Id,
                    currentUser.Role
                );
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [HttpGet("barcode/{barcode}")]
        public async Task<ActionResult<Product>> GetProductByBarcode(string barcode)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            var product = await _productService.GetProductByBarcodeAsync(
                barcode,
                currentUser?.Id,
                currentUser?.Role ?? Role.User
            );

            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveProduct(int id)
        {
            await _productService.ApproveProductAsync(id);
            return NoContent();
        }

        [HttpGet("unapproved")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Product>>> GetUnapprovedProducts()
        {
            var currentUser = _currentUserService.GetCurrentUser();
            Console.WriteLine($"Current user: {currentUser?.Id}, Role: {currentUser?.Role}");
            var products = await _productService.GetUnapprovedProductsAsync();
            return Ok(products);
        }
    }
}