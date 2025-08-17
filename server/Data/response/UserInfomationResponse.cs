using Entity.Models;

namespace Data.response
{
    public class UserInfomationResponse
    {
        public string UserId { get; set; }
        public string email { get; set; }
        public string fullName { get; set; }
        public bool IsEmailVerified { get; set; }
        public UserInformation userInformation { get; set; }
        public MacroTargets macroTargets { get; set; }
    }
}