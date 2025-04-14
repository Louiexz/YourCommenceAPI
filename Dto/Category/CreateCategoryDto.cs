using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Dto.Category
{
    public class CreateCategoryDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

        [NotMapped]
        public required List<IFormFile> Files { get; set; }
    }
}