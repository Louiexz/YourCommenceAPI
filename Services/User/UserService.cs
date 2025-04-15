using MongoDB.Driver;
using WebAPI.Data;
using WebAPI.Dto.User;
using WebAPI.View.User;
using WebAPI.models;

namespace WebAPI.Services.User
{
    public class UserService : IUserInterface
    {
        private readonly AppDbContext _context;
        private readonly IUserView _userView;
        
        public UserService(AppDbContext context, IUserView view){
            _context = context;
            _userView = view;
        }        

        public async Task<ResponseModel<UserModel>> DeleteUser(string Id)
        {
            var resposta = new ResponseModel<UserModel>();
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
            var resposta = new ResponseModel<UserModel>();
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
            var resposta = new ResponseModel<UserModel>();
            try{
                var user = await _context.Users.Find(bankUser => bankUser.Id == Id).FirstOrDefaultAsync();

                if (user == null)
                {
                    resposta.Message = $"User doesn't exist.";
                    return resposta;
                }
                user = _userView.UpdateUser(user, updateUser);

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
