using WebAPI.Dto.User;
using WebAPI.models;

namespace WebAPI.Services.User
{
    public interface IUserInterface
    {
        Task<ResponseModel<UserModel>> SignUp(CreateUserDto newUser);
        Task<ResponseModel<UserModel>> GetUser(int Id);
        Task<ResponseModel<UserModel>> SignIn(LoginDto userLogin);
        Task<ResponseModel<UserModel>> UpdateUser(int Id, UpdateUserDto newUser);
        Task<ResponseModel<UserModel>> DeleteUser(int Id);
    }
}