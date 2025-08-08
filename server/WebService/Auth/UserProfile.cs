using System.Security.Claims;
using Service.Dto.Response;
using Service.Interface;

namespace Webservices.Auth
{
    public class UserProfile
    {
        private readonly IUserService userService;
        public UserProfile(IUserService userService)
        {
            this.userService = userService;
        }
        public async Task<UserResponse?> GetUserDetail(ClaimsIdentity? claimsIdentity)
        {
            if (claimsIdentity is not null && !claimsIdentity.Claims.Any())
            {
                return null;
            }

            var emailClaim = claimsIdentity?.Claims.FirstOrDefault(e => e.Type.Contains("email"));
            if (emailClaim == null)
            {
                return null;
            }
            var requestorEmailId = emailClaim.Value;

            try
            {
                var data = await userService.GetByEmail(requestorEmailId);
                return data.StatusCodes == StatusCodes.Status200OK ? data.Data as UserResponse : null;
            }
            catch
            {
                return null;
            }
        }
    }
}