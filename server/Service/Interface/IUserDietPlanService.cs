using Service.Helpers;

namespace Service.Interface
{
    public interface IUserDietPlanService
    {
        Task<ApiResponse> GetByUserId(string userId);
    }
}