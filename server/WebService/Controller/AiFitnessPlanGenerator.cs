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
    public class AiFitnessPlanGeneratorController : ControllerBase
    {
        private readonly IAiService _aiService;
        private readonly UserProfile _userProfile;
        public AiFitnessPlanGeneratorController(IAiService aiService, UserProfile userProfile)
        {
            _aiService = aiService;
            _userProfile = userProfile;
        }

        [HttpPost("generate-fitness-and-nutrition-plan")]
        public async Task<IActionResult> GenerateFitnessPlan([FromBody] GenerateFitnessPlanRequest request)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userDetail = await _userProfile.GetUserDetail(claimsIdentity);
            if (userDetail is null)
            {
                return Unauthorized(new ApiResponse(null, "User profile not found", StatusCodes.Status404NotFound));
            }

            request.UserId = userDetail.Id;
            var response = await _aiService.GenerateFitnessPlan(request);
            if (response.Data is not null)
            {
                return Ok(response);
            }
            return BadRequest(response);

        }

        [HttpPost("basic-fitness-plan")]
        public async Task<IActionResult> GenerateBasicFitnessPlan()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userDetail = await _userProfile.GetUserDetail(claimsIdentity);
            if (userDetail is null)
            {
                return Unauthorized(new ApiResponse(null, "User profile not found", StatusCodes.Status404NotFound));
            }


            var response = await _aiService.GetBasicFitnessPlan(userDetail.Id);
            if (response.Data is not null)
            {
                return Ok(response);
            }
            return BadRequest(response);

        }
    }
}