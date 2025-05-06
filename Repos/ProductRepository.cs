using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Data.Context;
using ProductsApi.Models;

namespace ProductsApi.Repos
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        public ProductRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            var newProduct = _mapper.Map<Product>(product);
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return newProduct;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = await _mapper.ProjectTo<Product>(_context.Products).ToListAsync();
            return products;
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            var product = await _mapper.ProjectTo<Product>(_context.Products.Where(p => p.Id == id)).FirstOrDefaultAsync();

            try
            {
                if (product == null)
                {
                    throw new Exception($"Product with id {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return product;
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Product with id {id} not found.");
            }
        }

        public async Task<Product?> UpdateProductAsync(Guid id, Product product)
        {
            var existingProduct = await _mapper.ProjectTo<Product>(_context.Products.Where(p => p.Id == id)).FirstOrDefaultAsync();

            if (existingProduct != null)
            {
                _mapper.Map(product, existingProduct);
                await _context.SaveChangesAsync();
                return existingProduct;
            }
            else
            {
                throw new Exception($"Product with id {id} not found.");
            }

        }
    }
}