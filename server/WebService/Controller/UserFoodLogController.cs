using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto.Request;
using Service.Helpers;
using Service.Interface;
using Webservices.Auth;

namespace WebService.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserFoodLogController : ControllerBase
    {
        private readonly IUserFoodLogService _userFoodLogService;
        private readonly UserProfile _userProfile;
        public UserFoodLogController(IUserFoodLogService userFoodLogService, UserProfile userProfile)
        {
            _userFoodLogService = userFoodLogService;
            _userProfile = userProfile;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddFoodToLog([FromBody] CreateUserFoodLogRq req)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userDetail = await _userProfile.GetUserDetail(claimsIdentity);
            if (userDetail is null)
            {
                return Unauthorized(new ApiResponse(null, "User profile not found", StatusCodes.Status404NotFound));
            }

            req.UserId = userDetail.Id;
            return Ok(await _userFoodLogService.Create(req));
        }
    }
}