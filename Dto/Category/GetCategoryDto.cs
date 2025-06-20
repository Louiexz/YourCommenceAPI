namespace WebAPI.Dto.Category
{
    public class GetCategoryDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<string> ImagesId { get; set; }
    }
}