using WebAPI.Dto.Auth;
using WebAPI.Dto.User;
using WebAPI.models;

namespace WebAPI.Services.Auth
{
    public interface IAuthInterface
    {
        Task<ResponseModel<AuthResponseDto>> SignIn(LoginDto user);
        Task<ResponseModel<GetUserDto>> SignUp(CreateUserDto newUser);
        ResponseModel<string> LogOut(string token);
        string GenerateJwtToken(UserModel user);
    }
}