namespace WebAPI.Dto.User
{
    public class CreateUserDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Phone { get; set; } // Optional field
        public string? Cpf { get; set; } // Optional field
        public string? Country { get; set; } // Optional field
        public string? State { get; set; } // Optional field
        public string? City { get; set; } // Optional field
        public string? Street { get; set; } // Optional field
        public string? Complement { get; set; } // Optional field
    }
}