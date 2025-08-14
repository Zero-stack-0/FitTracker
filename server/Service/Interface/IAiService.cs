using Service.Dto.Request;
using Service.Helpers;

namespace Service.Interface
{
    public interface IAiService
    {
        Task<ApiResponse> GenerateFitnessPlan(GenerateFitnessPlanRequest req);
        Task<ApiResponse> GetBasicFitnessPlan(string loggedInUserId);
    }
}