using WebAPI.models;
using WebAPI.Dto.User;

namespace WebAPI.View.User
{
    public interface IUserView
    {
        public bool CheckPassword(UserModel registeredUser, string password);

        public UserModel CreateUser(CreateUserDto newUser);

        public UserModel UpdateUser(UserModel user, UpdateUserDto updateUser);
    }
}
