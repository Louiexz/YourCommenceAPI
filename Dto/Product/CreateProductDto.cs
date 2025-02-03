namespace WebAPI.Dto.Product
{
    public class CreateProductDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required double Price { get; set; }
        public double? Sale { get; set; }
        public required int Quantity { get; set; }
        public required int Category { get; set; }
    }
}