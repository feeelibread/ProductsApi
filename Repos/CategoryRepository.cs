using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Data.Context;
using ProductsApi.Models;

namespace ProductsApi.Repos
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        public CategoryRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            var newCategory = _mapper.Map<Category>(category);
            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return newCategory;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var categories = await _mapper.ProjectTo<Category>(_context.Categories).ToListAsync();
            return categories;

        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            var category = await _mapper.ProjectTo<Category>(_context.Categories.Where(c => c.Id == id)).FirstOrDefaultAsync();

            try
            {
                if (category == null)
                {
                    throw new Exception($"Category with id {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(int id, Category category)
        {
            var existingCategory = await _mapper.ProjectTo<Category>(_context.Categories.Where(c => c.Id == id)).FirstOrDefaultAsync();

            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                _context.Categories.Update(existingCategory);
                await _context.SaveChangesAsync();
                return existingCategory;
            }
            else
            {
                throw new Exception($"Category with id {id} not found.");
            }
        }

    }
}