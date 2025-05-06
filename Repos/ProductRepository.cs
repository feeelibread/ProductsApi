using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Data.Context;
using ProductsApi.Models;

namespace ProductsApi.Repos
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApiDbContext _context;
        public ProductRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            var newProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId
            };
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return newProduct;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            return product;
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Guid id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();
        }
    }
}