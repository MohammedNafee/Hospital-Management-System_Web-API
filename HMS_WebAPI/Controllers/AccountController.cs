using HMS_WebAPI.DTOs;
using HMS_WebAPI.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS_WebAPI.Controllers
{
    [Route("api/[Controller]/[action]")]
    public class AccountController : ControllerBase
    {
        AccountManager _accountManager;

        public AccountController(AccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddUser([FromBody] UserDTO user)
        {
            _accountManager.Register(user);
            return Created();
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDTO user)
        {
            var token = _accountManager.Authenticate(user); 

            if (token == null) 
                return Unauthorized();

            return Ok(token);
        }
    }
}
