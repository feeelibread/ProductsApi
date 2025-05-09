using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductsApi.DTOs.Requests;
using ProductsApi.DTOs.Responses;
using ProductsApi.Models;
using ProductsApi.Services;

namespace ProductsApi.Controllers
{
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(CategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            if (categories == null || !categories.Any())
            {
                return NotFound("No categories found.");
            }
            return Ok(categories);
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }
            return Ok(category);
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto categoryDto)
        {
            var existingCategories = await _categoryService.GetAllCategoriesAsync();
            if (existingCategories.Any(c => c.Name.Equals(categoryDto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest("Category with the same name already exists.");
            }
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryService.CreateCategoryAsync(categoryDto);
            var categoryResponse = _mapper.Map<CategoryResponse>(category);

            if (category == null)
            {
                return BadRequest("Category creation failed.");
            }
            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryResponse.Id }, category);
        }

        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto categoryDto)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }
            await _categoryService.UpdateCategoryAsync(id, categoryDto);
            return NoContent();
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }

        [HttpGet("GetCategoryByName/{name}")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var category = categories.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (category == null)
            {
                return NotFound($"Category with name {name} not found.");
            }
            return Ok(category);
        }
    }
}