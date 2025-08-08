using Microsoft.AspNetCore.Mvc;
using Service.Dto.Request;
using Service.Interface;

namespace WebService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid user data.");
            }

            var response = await _userService.CreateUser(request);

            return Ok(response);
        }
    }
}