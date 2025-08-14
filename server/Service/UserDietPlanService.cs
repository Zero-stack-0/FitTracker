using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repository.Interface;
using Service.Helpers;
using Service.Interface;

namespace Service
{
    public class UserDietPlanService : IUserDietPlanService
    {
        private readonly IFitnessAndnutritionPlansRepository _fitnessAndnutritionPlansRepository;
        public UserDietPlanService(IFitnessAndnutritionPlansRepository fitnessAndnutritionPlansRepository)
        {
            _fitnessAndnutritionPlansRepository = fitnessAndnutritionPlansRepository;
        }

        public async Task<ApiResponse> GetByUserId(string userId)
        {
            return new ApiResponse(await _fitnessAndnutritionPlansRepository.GetBasicPlanByUserId(userId), "Basic diet plan");
        }
    }
}