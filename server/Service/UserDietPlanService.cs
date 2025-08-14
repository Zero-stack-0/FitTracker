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
        private readonly IAiService _aiService;
        public UserDietPlanService(IFitnessAndnutritionPlansRepository fitnessAndnutritionPlansRepository, IAiService aiService)
        {
            _fitnessAndnutritionPlansRepository = fitnessAndnutritionPlansRepository;
            _aiService = aiService;
        }

        public async Task<ApiResponse> GetByUserId(string userId)
        {
            var dietPlan = await _fitnessAndnutritionPlansRepository.GetBasicPlanByUserId(userId);
            if (dietPlan is null)
            {
                return await _aiService.GetBasicFitnessPlan(userId);
            }
            return new ApiResponse(dietPlan, "Basic diet plan");
        }
    }
}