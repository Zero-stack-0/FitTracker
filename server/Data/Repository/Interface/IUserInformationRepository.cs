

using Entity.Models;

namespace Data.Repository.Interface
{
    public interface IUserInformationRepository
    {
        Task<UserInformation?> Create(UserInformation userInformation);
        Task<UserInformation?> GetByUserId(string userId);
        Task<bool> Update(UserInformation userInformation);
    }
}