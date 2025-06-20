using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.models
{
    public enum UserType
    {
        Cliente,
        Admin
    }
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.
        public string Id { get; set; }
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.

        [BsonElement("username")]
        [Required(ErrorMessage = "Username is required.")]
        public required string Username { get; set; }

        [BsonElement("email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public required string Email { get; set; }

        [BsonElement("password")]
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string Password { get; set; }

        [BsonElement("phone")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? Phone { get; set; }

        [BsonElement("cpf")]
        [Required(ErrorMessage = "CPF is required.")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "Invalid CPF format.")]
        public string? Cpf { get; set; }

        [BsonElement("country")]
        public string? Country { get; set; }

        [BsonElement("state")]
        public string? State { get; set; }

        [BsonElement("city")]
        public string? City { get; set; }

        [BsonElement("street")]
        public string? Street { get; set; }

        [BsonElement("complement")]
        public string? Complement { get; set; }

        [BsonRepresentation(BsonType.String)]
        [Required(ErrorMessage = "Type is required.")]
        [BsonElement("type")]
        public UserType Type { get; set; } = UserType.Cliente;

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
