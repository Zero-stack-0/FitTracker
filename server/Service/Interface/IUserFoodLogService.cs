using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.Dto.Request;
using Service.Helpers;

namespace Service.Interface
{
    public interface IUserFoodLogService
    {
        Task<ApiResponse> Create(CreateUserFoodLogRq req);
        Task<ApiResponse> GetRecentFoodLogEntriesAsync(string userId);
        Task<ApiResponse> GetFoodLogHistory(string userId, int weekOffset);
    }
}