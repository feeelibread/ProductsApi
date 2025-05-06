using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Data.Context;
using ProductsApi.Models;

namespace ProductsApi.Repos
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApiDbContext _context;
        public CategoryRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            var newCategory = new Category
            {
                Name = category.Name,
                Description = category.Description
            };
            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return newCategory;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;

        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(int id, Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;
            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();
            return existingCategory;

        }

    }
}