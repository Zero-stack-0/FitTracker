using System.Security.Claims;
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
    public class UserDietPlanController : ControllerBase
    {
        private readonly IUserDietPlanService _userDietPlanService;
        private readonly UserProfile _userProfile;
        public UserDietPlanController(IUserDietPlanService userDietPlanService, UserProfile userProfile)
        {
            _userDietPlanService = userDietPlanService;
            _userProfile = userProfile;
        }

        [HttpGet("basic-diet-plan")]
        public async Task<IActionResult> GetBasicDietPlan()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userDetail = await _userProfile.GetUserDetail(claimsIdentity);
            if (userDetail is null)
            {
                return Unauthorized(new ApiResponse(null, "User profile not found", StatusCodes.Status404NotFound));
            }

            return Ok(await _userDietPlanService.GetByUserId(userDetail.Id));
        }

        [HttpPut("user-information-diet")]
        public async Task<IActionResult> UpdateDietPlan([FromBody] UpdateUserFitnessGoalReq req)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userDetail = await _userProfile.GetUserDetail(claimsIdentity);
            if (userDetail is null)
            {
                return Unauthorized(new ApiResponse(null, "User profile not found", StatusCodes.Status404NotFound));
            }

            req.UserId = userDetail.Id;

            return Ok(await _userDietPlanService.UpdateUserBasicDietPlan(req));
        }

        [HttpGet("can-update-diet-plan")]
        public async Task<IActionResult> CanUserUpdateDietPlan()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userDetail = await _userProfile.GetUserDetail(claimsIdentity);
            if (userDetail is null)
            {
                return Unauthorized(new ApiResponse(null, "User profile not found", StatusCodes.Status404NotFound));
            }

            return Ok(await _userDietPlanService.CanUserUpdateDietPlan(userDetail.Id));
        }
    }
}