using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Dto.User;
using WebAPI.Services.User;
using WebAPI.models;

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
        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<ResponseModel<UserModel>>> GetUser(string Id){
            var user = await _userInterface.GetUser(Id);
            return Ok(user);
        }
        [Authorize]
        [HttpPatch("user")]
        public async Task<ActionResult<ResponseModel<UserModel>>> UpdateUser(string Id, UpdateUserDto updateUser){
            var user = await _userInterface.UpdateUser(Id, updateUser);
            return Ok(user);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("user")]
        public async Task<ActionResult<ResponseModel<UserModel>>> DeleteUser(string Id){
            var user = await _userInterface.DeleteUser(Id);
            return Ok(user);
        }
    }
}