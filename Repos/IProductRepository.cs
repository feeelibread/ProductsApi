using ProductsApi.Models;

namespace ProductsApi.Repos
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(Guid id);
        Task<Product> CreateProductAsync(Product product);
        Task UpdateProductAsync(Guid id, Product product);
        Task DeleteProductAsync(Guid id);

    }
}