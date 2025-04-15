using WebAPI.models;
using WebAPI.Dto.User;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.View.User
{
    public class UserView(IPasswordHasher<UserModel> passwordHasher) : IUserView
    {
        private readonly IPasswordHasher<UserModel> _passwordHasher = passwordHasher;

        public bool CheckPassword(UserModel registeredUser, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(
                registeredUser, registeredUser.Password, password);

            return result != PasswordVerificationResult.Failed;
        }

        public UserModel CreateUser(CreateUserDto newUser)
        {
            var user = new UserModel
            {
                Username = newUser.Username, // Se for ValueObject
                Email = newUser.Email,
                Password = ""
            };

            foreach (var property in newUser.GetType().GetProperties())
            {
                var newValue = property.GetValue(newUser);
                if (newValue != null)
                {
                    var userProperty = user.GetType().GetProperty(property.Name);
                    if (userProperty != null && userProperty.CanWrite
                        && property.Name != nameof(UserModel.Password)
                        && property.Name != nameof(UserModel.Username)
                        && property.Name != nameof(UserModel.Email)
                        )
                    {
                        // Ignora a propriedade Password
                        // userProperty.SetValue(user, newValue);
                    }
                    else if (userProperty != null && userProperty.CanWrite)
                    {
                        userProperty.SetValue(user, newValue);
                    }
                }
            }
            
            if (!string.IsNullOrEmpty(newUser.Password))
            {
                user.Password = _passwordHasher.HashPassword(user, newUser.Password);
            }

            return user;
        }

        public UserModel UpdateUser(UserModel user, UpdateUserDto updateUser)
        {
            foreach (var property in updateUser.GetType().GetProperties())
            {
                var newValue = property.GetValue(updateUser);
                if (newValue != null)
                {
                    var userProperty = user.GetType().GetProperty(property.Name);
                    if (userProperty != null && userProperty.CanWrite)
                    {
                        userProperty.SetValue(user, newValue);
                    }
                }
            }
            user.UpdatedAt = DateTime.UtcNow;
            return user;
        }
    }
}
