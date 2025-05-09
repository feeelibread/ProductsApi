using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProductsApi.DTOs.Requests;
using ProductsApi.DTOs.Responses;
using ProductsApi.Models;
using ProductsApi.Repos;

namespace ProductsApi.Services
{
    public class CategoryService
    {
        //TODO: Improve error handling
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<List<CategoryResponse>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return _mapper.Map<List<CategoryResponse>>(categories);
        }
        public async Task<CategoryResponse> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found.");
            }
            return _mapper.Map<CategoryResponse>(category);
        }
        public async Task<CreateCategoryDto> CreateCategoryAsync(CreateCategoryDto categoryDto)
        {
            // Check if the category already exists
            var existingCategories = await _categoryRepository.GetAllCategoriesAsync();
            if (existingCategories.Any(c => c.Name.Equals(categoryDto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Category with the same name already exists.");
            }
            var category = _mapper.Map<Category>(categoryDto);
            var createdCategory = await _categoryRepository.CreateCategoryAsync(category);
            return _mapper.Map<CreateCategoryDto>(createdCategory);
        }
        public async Task UpdateCategoryAsync(int id, UpdateCategoryDto categoryDto)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found.");
            }
            _mapper.Map(categoryDto, category);
            await _categoryRepository.UpdateCategoryAsync(id, category);

        }
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found.");
            }
            await _categoryRepository.DeleteCategoryAsync(id);
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _categoryRepository.CategoryExistsAsync(id);
        }


    }
}