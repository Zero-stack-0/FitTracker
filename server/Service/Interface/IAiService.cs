using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.Models;
using Service.Dto.Request;
using Service.Helpers;

namespace Service.Interface
{
    public interface IAiService
    {
        Task<ApiResponse> GenerateFitnessPlan(GenerateFitnessPlanRequest req);
    }
}