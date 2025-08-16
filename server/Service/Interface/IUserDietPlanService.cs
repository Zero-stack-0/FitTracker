using Service.Dto.Request;
using Service.Helpers;

namespace Service.Interface
{
    public interface IUserDietPlanService
    {
        Task<ApiResponse> GetByUserId(string userId);
        Task<ApiResponse> UpdateUserBasicDietPlan(UpdateUserFitnessGoalReq req);
        Task<ApiResponse> CanUserUpdateDietPlan(string userId);
    }
}