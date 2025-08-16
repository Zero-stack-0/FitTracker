using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers;
using Service.Interface;
using Webservices.Auth;

namespace WebService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotivationController : ControllerBase
    {
        private readonly IMotivationService _motivationService;
        private readonly UserProfile _userProfile;
        public MotivationController(IMotivationService motivationService, UserProfile userProfile)
        {
            _motivationService = motivationService;
            _userProfile = userProfile;
        }

        [HttpGet]
        public async Task<IActionResult> GetMotivation()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userDetail = await _userProfile.GetUserDetail(claimsIdentity);
            if (userDetail is null)
            {
                return Unauthorized(new ApiResponse(null, "User profile not found", StatusCodes.Status404NotFound));
            }

            return Ok(await _motivationService.GetMotivation());
        }
    }
}