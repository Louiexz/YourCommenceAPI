using WebAPI.Dto.User;
using WebAPI.models;

namespace WebAPI.Services.User
{
    public interface IUserInterface
    {
        Task<ResponseModel<UserModel>> GetUser(string Id);
        Task<ResponseModel<UserModel>> UpdateUser(string Id, UpdateUserDto newUser);
        Task<ResponseModel<UserModel>> DeleteUser(string Id);
    }
}
