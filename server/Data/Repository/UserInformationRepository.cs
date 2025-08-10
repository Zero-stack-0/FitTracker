using Data.Repository.Interface;
using Entity.Models;
using MongoDB.Driver;

namespace Data.Repository
{
    public class UserInformationRepository : IUserInformationRepository
    {
        private readonly IMongoCollection<UserInformation> _userInformationCollection;
        public UserInformationRepository(FitTrackerDbContext context)
        {
            _userInformationCollection = context.UserInformation;
        }

        public async Task<UserInformation?> Create(UserInformation userInformation)
        {
            return await _userInformationCollection.InsertOneAsync(userInformation).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    return null;
                }
                return userInformation;
            });
        }
    }
}