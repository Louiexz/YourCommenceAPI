using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using MongoDB.Driver;
using Microsoft.Extensions.Caching.Memory;
using WebAPI.Data;
using WebAPI.models;
using WebAPI.Dto.User;
using WebAPI.Dto.Auth;
using WebAPI.View.User;

namespace WebAPI.Services.Auth
{
    public class AuthService : IAuthInterface
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private readonly IUserView _userView;

        public AuthService(
            AppDbContext context, IConfiguration config,
            IMemoryCache cache, IUserView view)
        {
            _context = context;
            _config = config;
            _cache = cache;
            _userView = view;
        }
        
        public async Task<ResponseModel<AuthResponseDto>> SignIn(LoginDto userLogin)
        {
            var resposta = new ResponseModel<AuthResponseDto>();

            try
            {
                UserModel registeredUser;

                if (userLogin.Credential.Contains("@"))
                {
                    registeredUser = await _context.Users
                        .Find(u => u.Email == userLogin.Credential)
                        .FirstOrDefaultAsync();
                }
                else
                {
                    registeredUser = await _context.Users
                        .Find(u => u.Username == userLogin.Credential)
                        .FirstOrDefaultAsync();
                }

                if (registeredUser == null)
                {
                    resposta.Message = "User does not exist.";
                    resposta.Status = false;
                    return resposta;
                }

                // Hash da senha fornecida
                var result = _userView.CheckPassword(registeredUser, userLogin.Password);

                if (!result)
                {
                    resposta.Message = "Wrong password or credential.";
                    resposta.Status = false;
                    return resposta;
                }

                var token = GenerateJwtToken(registeredUser);

                resposta.Data = new AuthResponseDto
                {
                    Token = token,
                    User = new GetUserDto {
                        Username = registeredUser.Username,
                        Email = registeredUser.Email,
                    }
                };
                resposta.Message = $"User {registeredUser.Username} logged in successfully.";
                resposta.Status = true;
                return resposta;
            }
            catch (Exception e)
            {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }
        public async Task<ResponseModel<GetUserDto>> SignUp(CreateUserDto newUser)
        {
            var resposta = new ResponseModel<GetUserDto>();
            try{
                var checkUserEmail = await _context.Users
                    .Find(u => u.Email == newUser.Email)
                    .FirstOrDefaultAsync();

                if (checkUserEmail != null){
                    resposta.Message = $"Email already taken.";    
                    resposta.Status = false;
                    return resposta;
                }
                var checkUserUsername = await _context.Users
                    .Find(u => u.Username == newUser.Username)
                    .FirstOrDefaultAsync();

                if (checkUserUsername != null){
                    resposta.Message = $"Username already taken.";
                    resposta.Status = false; 
                    return resposta;
                }

                UserModel user = _userView.CreateUser(newUser);

                user.Type = user.Type == UserType.Admin ? UserType.Cliente : user.Type;

                await _context.Users.InsertOneAsync(user);

                resposta.Message = $"User created sucessfully.";
                resposta.Data = new GetUserDto {
                    Username = newUser.Username,
                    Email = newUser.Email,
                };
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public ResponseModel<string> LogOut(string token)
        {
            var resposta = new ResponseModel<string>();
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtTokenHandler.ReadJwtToken(token);

                if (jwtToken == null)
                {
                    resposta.Message = "Invalid token.";
                    resposta.Status = false;
                    return resposta;
                }

                var expiration = jwtToken.ValidTo;
                _cache.Set(token, true, expiration - DateTime.UtcNow);

                resposta.Message = "User logged out successfully.";
                resposta.Status = true;
                resposta.Data = "Success";
                return resposta;
            }
            catch (Exception e)
            {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public string GenerateJwtToken(UserModel user)
        {
            var secretKey = _config["JwtSettings:SecretKey"];
            var issuer = _config["JwtSettings:Issuer"];
            var audience = _config["JwtSettings:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("username", user.Username),
                new Claim("UserType", user.Type.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}