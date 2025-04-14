using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.models
{
    public class ProductModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }
        
        [BsonElement("price")]
        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public double Price { get; set; }

        [BsonElement("sale")]
        [Required(ErrorMessage = "Sale is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Sale must be a positive number.")]
        public double? Sale { get; set; }

        [BsonElement("quantity")]
        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int Quantity { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("category")]
        [Required(ErrorMessage = "Category is required")]
        public required List<CategoryModel> Category { get; set; }

        [BsonElement("image")]
        [Required(ErrorMessage = "Images are required")]
        public required List<string> ImagesId { get; set; }
    }
}
