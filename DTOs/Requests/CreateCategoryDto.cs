using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApi.DTOs.Requests
{
    public class CreateCategoryDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}