using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto.Request;
using Service.Dto.Response;
using Service.Helpers;
using Service.Interface;
using Webservices.Auth;

namespace WebService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly GenerateJwtToken generateJwtToken;
        private readonly UserProfile _userProfile;
        public UserController(IUserService userService, GenerateJwtToken generateJwtToken, UserProfile userProfile)
        {
            _userService = userService;
            this.generateJwtToken = generateJwtToken;
            _userProfile = userProfile;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid user data.");
            }

            var response = await _userService.CreateUser(request);
            if (response.StatusCodes == (int)HttpStatusCode.Created)
            {
                UserResponse userResponse = response.Data as UserResponse ?? new UserResponse();
                response.Data = generateJwtToken.GenerateToken(userResponse.Role.ToString(), userResponse.Email);
                return Ok(response);
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid login data.");
            }

            var response = await _userService.Login(request);
            if (response.StatusCodes == (int)HttpStatusCode.OK)
            {
                UserResponse userResponse = response.Data as UserResponse ?? new UserResponse();
                response.Data = generateJwtToken.GenerateToken(userResponse.Role.ToString(), userResponse.Email);
                return Ok(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userDetail = await _userProfile.GetUserDetail(claimsIdentity);
            if (userDetail == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new ApiResponse
            {
                Data = userDetail,
                Message = "User profile retrieved successfully",
                StatusCodes = (int)HttpStatusCode.OK
            });
        }
    }
}