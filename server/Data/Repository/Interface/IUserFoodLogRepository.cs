using Data.Response;
using Entity.DbModels;

namespace Data.Repository.Interface
{
    public interface IUserFoodLogRepository
    {
        Task<UserFoodLog?> Create(UserFoodLog userFoodLog);
        Task<List<object>> GetRecentFoodLogEntriesAsync(string userId);
        Task<UserFoodLogWeekHistory> GetFoodLogEntriesByStartAndEndDate(string userId, DateTime startDate, DateTime endDate);
    }
}