using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _auth;
        public AuthController(AuthService auth)
        {
            _auth = auth;   
        }


        [HttpPost("register")]
        public ActionResult<User> Register(User user)
        {
            User newUser= _auth.Register(user);
            return Ok(newUser);
            
        }


        [HttpPost("login")]
        public ActionResult<User> Login(string username,string password)
        {
            var userToken = _auth.login(username, password);
            if (userToken != null)
            {
                return Ok(userToken);
            }

            return Unauthorized("Invalid username or password");

        }

    }
}
