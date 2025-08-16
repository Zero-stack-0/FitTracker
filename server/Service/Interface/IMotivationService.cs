using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.Helpers;

namespace Service.Interface
{
    public interface IMotivationService
    {
        Task<ApiResponse> GetMotivation();
    }
}