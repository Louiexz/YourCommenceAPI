using MongoDB.Driver;
using WebAPI.Data;
using WebAPI.models;
using WebAPI.Dto.User;

namespace WebAPI.Services.User
{
    public class UserService : IUserInterface
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<UserModel>> DeleteUser(string Id)
        {
            ResponseModel<UserModel> resposta = new ResponseModel<UserModel>();
            try{
                var user = await _context.Users.DeleteOneAsync(bankUser => bankUser.Id == Id);

                if (user.DeletedCount == 0){
                    resposta.Message = $"User don't exist.";    
                    return resposta;
                }
                resposta.Message = $"User deleted successfully.";
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<UserModel>> GetUser(string Id)
        {
            ResponseModel<UserModel> resposta = new ResponseModel<UserModel>();
            try{
                var user = await _context.Users.Find(bankCategory => bankCategory.Id == Id).FirstOrDefaultAsync();

                if (user == null){
                    resposta.Message = $"User don't exist.";    
                    return resposta;
                }
                user.Password = "";
                resposta.Data = user;
                resposta.Message = $"User {user.Username} profile.";
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<UserModel>> UpdateUser(string Id, UpdateUserDto updateUser)
        {
            ResponseModel<UserModel> resposta = new ResponseModel<UserModel>();
            try{
                var user = await _context.Users.Find(bankUser => bankUser.Id == Id).FirstOrDefaultAsync();

                if (user == null)
                {
                    resposta.Message = $"User doesn't exist.";
                    return resposta;
                }

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
                await _context.Users.ReplaceOneAsync(bankUser => bankUser.Id == Id, user);

                resposta.Message = $"User updated sucessfully.";
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }
    }
}
