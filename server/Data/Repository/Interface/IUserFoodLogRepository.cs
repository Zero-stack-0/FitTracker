using Entity.DbModels;

namespace Data.Repository.Interface
{
    public interface IUserFoodLogRepository
    {
        Task<UserFoodLog?> Create(UserFoodLog userFoodLog);
        Task<List<object>> GetRecentFoodLogEntriesAsync(string userId);
    }
}