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

        public async Task<UserInformation?> GetByUserId(string userId)
        {
            return await _userInformationCollection.Find(u => u.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<bool> Update(UserInformation userInformation)
        {
            var filter = Builders<UserInformation>.Filter.Eq(p => p.UserId, userInformation.UserId);
            var update = Builders<UserInformation>.Update
                .Set(p => p.ActivityLevel, userInformation.ActivityLevel)
                .Set(p => p.FitnessGoal, userInformation.FitnessGoal)
                .Set(p => p.Age, userInformation.Age)
                .Set(p => p.Gender, userInformation.Gender)
                .Set(p => p.Height, userInformation.Height)
                .Set(p => p.Weight, userInformation.Weight)
                .Set(p => p.UpdatedAt, DateTime.UtcNow);

            var result = await _userInformationCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }
    }
}