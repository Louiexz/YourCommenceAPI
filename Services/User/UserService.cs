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
        public async Task<ResponseModel<UserModel>> SignUp(CreateUserDto newUser)
        {
            ResponseModel<UserModel> resposta = new ResponseModel<UserModel>();
            try{
                var checkUserEmail = await _context.Users.Find(bankCategory => bankCategory.Email == newUser.Email).FirstOrDefaultAsync();

                if (checkUserEmail != null){
                    resposta.Message = $"Email already taken.";    
                    return resposta;
                }
                var checkUserUsername = await _context.Users.Find(bankCategory => bankCategory.Username == newUser.Username).FirstOrDefaultAsync();

                if (checkUserUsername != null){
                    resposta.Message = $"Username already taken.";    
                    return resposta;
                }

                var user = new UserModel{
                    Username = newUser.Username,
                    Password = newUser.Password,
                    Email = newUser.Email,
                };
                await _context.Users.InsertOneAsync(user);

                resposta.Message = $"User created sucessfully.";
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<UserModel>> DeleteUser(int Id)
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

        public async Task<ResponseModel<UserModel>> GetUser(int Id)
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
        public async Task<ResponseModel<UserModel>> SignIn(LoginDto userLogin)
        {
            ResponseModel<UserModel> resposta = new ResponseModel<UserModel>();
            try{
                UserModel registeredUser;

                if (userLogin.Credential.Contains("@")) {
                    registeredUser = await _context.Users
                        .Find(bankCategory => bankCategory.Email == userLogin.Credential).FirstOrDefaultAsync();
                } else {
                    registeredUser = await _context.Users
                        .Find(bankCategory => bankCategory.Username == userLogin.Credential).FirstOrDefaultAsync();
                }

                if (registeredUser == null){
                    resposta.Message = $"User don't exist.";    
                    return resposta;
                }
                registeredUser = await _context.Users.Find(bankCategory => bankCategory.Password == userLogin.Password).FirstOrDefaultAsync();
                
                if (registeredUser == null){
                    resposta.Message = $"Wrong password or credential.";    
                    return resposta;
                }

                resposta.Message = $"User {registeredUser.Username} logged succesfully.";
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<UserModel>> UpdateUser(int Id, UpdateUserDto updateUser)
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