using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Dto.Category
{
    public class UpdateCategoryDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        
        [NotMapped]
        public List<IFormFile>? Files { get; set; }
    }
}