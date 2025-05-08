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
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly IMapper _mapper;
        public ProductController(ProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            var existingProducts = await _productService.GetAllProductsAsync();
            if (existingProducts.Any(c => c.Name.Equals(productDto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest("Product with the same name already exists.");
            }
            var product = _mapper.Map<Product>(productDto);
            await _productService.CreateProductAsync(productDto);

            var productResponse = _mapper.Map<ProductResponse>(product);

            if (product == null)
            {
                return BadRequest("Category creation failed.");
            }
            return CreatedAtAction(nameof(GetProductById), new { id = productResponse.Id }, product);
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }
            return Ok(products);
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return Ok(product);
        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] CreateProductDto productDto)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            await _productService.UpdateProductAsync(id, productDto);
            return NoContent();
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}