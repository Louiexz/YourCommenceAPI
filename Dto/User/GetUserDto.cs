namespace WebAPI.Dto.User
{
    public class GetUserDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
    }
}