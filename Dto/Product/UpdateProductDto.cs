using System.ComponentModel.DataAnnotations.Schema;
namespace WebAPI.Dto.Product
{
    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public double? Sale { get; set; }
        public int? Quantity { get; set; }
        public string? Category { get; set; }

        [NotMapped]
        public List<IFormFile>? Files { get; set; }
    }
}