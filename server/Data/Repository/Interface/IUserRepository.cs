using Data.response;
using Entity.Models;

namespace Data.Repository.Interface
{
    public interface IUserRepository
    {
        Task<Users?> Create(Users user);
        Task<Users?> GetByEmail(string email);
        Task<Users?> Login(string email, string password);
        Task<UserInfomationResponse> GetUserInformation(string email);
        Task<Users?> GetById(string id);
        Task<bool> Update(Users user);
        Task<Users?> GetByVerificationCode(string code);
        Task<bool> UpdateEmailVerification(Users user);
        Task<bool> UpdateIsEmailSentFlag(Users user);
    }
}