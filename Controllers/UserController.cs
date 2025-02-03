using Microsoft.AspNetCore.Mvc;
using WebAPI.Dto.User;
using WebAPI.models;
using WebAPI.Services.User;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserInterface _userInterface;
    
        public UserController(IUserInterface userInterface)
        {
            _userInterface = userInterface;
        }
        [HttpPost("SignIn")]
        public async Task<ActionResult<ResponseModel<UserModel>>> SignIn(LoginDto userLogin){
            var user = await _userInterface.SignIn(userLogin);
            return Ok(user);
        }
        [HttpGet("GetUser")]
        public async Task<ActionResult<ResponseModel<UserModel>>> GetUser(int Id){
            var user = await _userInterface.GetUser(Id);
            return Ok(user);
        }
        [HttpPost("SignUp")]
        public async Task<ActionResult<ResponseModel<UserModel>>> SignUp(CreateUserDto newUser){
            var user = await _userInterface.SignUp(newUser);
            return Ok(user);
        }
        [HttpPatch("UpdateUser")]
        public async Task<ActionResult<ResponseModel<UserModel>>> UpdateUser(int Id, UpdateUserDto updateUser){
            var user = await _userInterface.UpdateUser(Id, updateUser);
            return Ok(user);
        }
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult<ResponseModel<UserModel>>> DeleteUser(int Id){
            var user = await _userInterface.DeleteUser(Id);
            return Ok(user);
        }
    }
}