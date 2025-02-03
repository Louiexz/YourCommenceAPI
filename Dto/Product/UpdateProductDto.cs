using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Dto.Product
{
    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public double? Sale { get; set; }
        public int? Quantity { get; set; }
        public int? Category { get; set; }
    }
}