using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Models;
using Services;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService; 
        }
        [HttpGet]
        public async Task <ActionResult<ICollection<User>>> getAllUsers()
        {
            var users=await _userService.GetUsers();
            return Ok(users);
        }
        [HttpPut("user/{id}")]
        public async Task<ActionResult<ICollection<User>>> updateUserDetails(int id,User newUser)
        {
            var updatedUser = await _userService.updateUser(id,newUser);
            return Ok(updatedUser);
        }

    }
}
