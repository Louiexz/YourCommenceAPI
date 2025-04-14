using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dto.Auth;
using WebAPI.Dto.User;
using WebAPI.models;
using WebAPI.Services.Auth;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface _authInterface;
    
        public AuthController(IAuthInterface authInterface)
        {
            _authInterface = authInterface;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ResponseModel<UserModel>>> SignIn(LoginDto userLogin)
        {
            var user = await _authInterface.SignIn(userLogin);
            return Ok(user);
        }
        [AllowAnonymous]
        [HttpPost("user")]
        public async Task<ActionResult<ResponseModel<UserModel>>> SignUp(CreateUserDto newUser)
        {
            var user = await _authInterface.SignUp(newUser);
            return Ok(user);
        }
        [Authorize]
        [HttpPost("logout")]
        public ActionResult<ResponseModel<string>> LogOut([FromBody] string token)
        {
            var result = _authInterface.LogOut(token);
            return Ok(result);
        }
    }
}