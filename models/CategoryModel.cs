using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.models
{
    public class CategoryModel
    {
        [BsonId]
        public int Id { get; set; }
        [BsonElement("name")]
        [Required(ErrorMessage = "name is required.")]
#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.
        public string Name { get; set; }
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}