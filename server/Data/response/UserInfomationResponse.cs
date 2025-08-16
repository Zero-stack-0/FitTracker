using Entity.Models;
using MongoDB.Bson;

namespace Data.response
{
    public class UserInfomationResponse
    {
        public string UserId { get; set; }
        public string email { get; set; }
        public string fullName { get; set; }
        public UserInformation userInformation { get; set; }
        public MacroTargets macroTargets { get; set; }
    }
}