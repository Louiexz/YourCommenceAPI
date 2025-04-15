using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using WebAPI.Dto.Category;

namespace WebAPI.models
{
    public class CategoryModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.        
        public string Id { get; set; }
#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.

        [BsonElement("name")]
        [Required(ErrorMessage = "name is required.")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("image")]
        [Required(ErrorMessage = "Images are required")]
        public List<string> ImagesId { get; set; }

        public static implicit operator CategoryModel(GetCategoryDto v)
        {
            throw new NotImplementedException();
        }
    }
}
