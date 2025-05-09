using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.DTOs.Requests;
using ProductsApi.DTOs.Responses;
using ProductsApi.Models;
using ProductsApi.Repos;

namespace ProductsApi.Services
{
    public class ProductService
    {
        //TODO: Improve error handling
        private readonly IProductRepository _productRepository;
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper, CategoryService categoryService)
        {
            _categoryService = categoryService;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            var productDtos = _mapper.Map<List<ProductResponse>>(products);
            return productDtos;
        }

        public async Task<ProductResponse> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }
            var productDto = _mapper.Map<ProductResponse>(product);

            return productDto;
        }
        public async Task<CreateProductDto> CreateProductAsync(CreateProductDto productDto)
        {
            // Check if the product already exists
            var existingProduct = await _productRepository.GetAllProductsAsync();
            if (existingProduct.Any(p => p.Name == productDto.Name && p.CategoryId == productDto.CategoryId))
            {
                throw new Exception("Product already exists in the specified category.");
            }

            // Check if the category exists
            var categoryExists = await _categoryService.CategoryExistsAsync(productDto.CategoryId);
            if (!categoryExists)
            {
                throw new Exception("Category does not exist.");
            }

            var product = _mapper.Map<Product>(productDto);
            var createdProduct = await _productRepository.CreateProductAsync(product);
            var productResponse = _mapper.Map<CreateProductDto>(createdProduct);
            return productResponse;

        }

        public async Task UpdateProductAsync(Guid id, UpdateProductDto productDto)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }
            _mapper.Map(productDto, product);
            await _productRepository.UpdateProductAsync(id, product);
        }
        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }
            await _productRepository.DeleteProductAsync(id);

        }
    }
}