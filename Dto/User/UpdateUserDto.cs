namespace WebAPI.Dto.User
{
    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; } // Optional field
        public string? Cpf { get; set; } // Optional field
        public string? State { get; set; } // Optional field
        public string? Street { get; set; } // Optional field
        public string? Complement { get; set; } // Optional field
    }
}