using WebAPI.Dto.User;

namespace WebAPI.Dto.Auth
{
    public class AuthResponseDto
    {
        public required string Token { get; set; }
        public required GetUserDto User { get; set; }
    }
}