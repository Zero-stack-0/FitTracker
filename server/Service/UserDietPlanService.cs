using Data.Repository.Interface;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Service.Dto.Request;
using Service.Helpers;
using Service.Interface;

namespace Service
{
    public class UserDietPlanService : IUserDietPlanService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserInformationRepository _userInformationRepository;
        private readonly IFitnessAndnutritionPlansRepository _fitnessAndnutritionPlansRepository;
        private readonly IAiService _aiService;
        public UserDietPlanService(IUserRepository userRepository, IUserInformationRepository userInformationRepository, IFitnessAndnutritionPlansRepository fitnessAndnutritionPlansRepository, IAiService aiService)
        {
            _userRepository = userRepository;
            _userInformationRepository = userInformationRepository;
            _fitnessAndnutritionPlansRepository = fitnessAndnutritionPlansRepository;
            _aiService = aiService;
        }

        public async Task<ApiResponse> GetByUserId(string userId)
        {
            var userInformation = await _userInformationRepository.GetByUserId(userId);
            if (userInformation is null)
            {
                return new ApiResponse(null, "Invalid userid", StatusCodes.Status400BadRequest);
            }

            var user = await _userRepository.GetById(userId);
            if (user is null)
            {
                return new ApiResponse(null, "Invalid requestor id", StatusCodes.Status403Forbidden);
            }

            if (!user.IsEmailVerified)
            {
                return new ApiResponse(null, "Please verify your email to generate diet plan", StatusCodes.Status400BadRequest);
            }

            var dietPlan = await _fitnessAndnutritionPlansRepository.GetBasicPlanByUserId(userId);
            if (dietPlan is null)
            {
                return await _aiService.GetBasicFitnessPlan(userId);
            }

            return new ApiResponse(dietPlan, "Basic diet plan");
        }

        //under progress
        public async Task<ApiResponse> UpdateUserBasicDietPlan(UpdateUserFitnessGoalReq req)
        {
            var userId = req.UserId ?? "";

            var user = await _userRepository.GetById(userId);
            if (user == null)
                return new ApiResponse(null, "Invalid user", StatusCodes.Status400BadRequest);

            if (!user.IsEmailVerified)
            {
                return new ApiResponse(null, "Please verify your email, to update your email", StatusCodes.Status400BadRequest);
            }

            var userInfo = await _userInformationRepository.GetByUserId(userId);
            if (userInfo == null)
                return new ApiResponse(null, "Invalid user ID", StatusCodes.Status400BadRequest);

            bool needToUpdateDietPlan = NeedToUpdateDietPlan(req, userInfo);
            bool hasExceededLimit = await HasUserExceededDietPlanLimit(user.Id);

            if (needToUpdateDietPlan && hasExceededLimit)
                return new ApiResponse(null, "You can only update your diet plan twice a month", StatusCodes.Status400BadRequest);

            // Update name if changed
            if (user.FullName != req.FullName)
            {
                user.FullName = req.FullName;
                if (!await _userRepository.Update(user))
                    return new ApiResponse(null, "Error updating user", StatusCodes.Status500InternalServerError);
            }

            // Update user info
            userInfo.Age = req.Age;
            userInfo.Height = req.Height;

            if (!hasExceededLimit)
            {
                userInfo.Weight = req.Weight ?? userInfo.Weight;
                userInfo.ActivityLevel = req.ActivityLevel ?? userInfo.ActivityLevel;
                userInfo.FitnessGoal = req.FitnessGoal ?? userInfo.FitnessGoal;
            }

            if (!await _userInformationRepository.Update(userInfo))
                return new ApiResponse(null, "Error updating user information", StatusCodes.Status500InternalServerError);

            if (!needToUpdateDietPlan)
                return new ApiResponse(userInfo, "User profile updated successfully");

            var existingPlan = await _fitnessAndnutritionPlansRepository.GetBasicPlanByUserId(userId);

            if (existingPlan != null)
            {
                bool deleted = await _fitnessAndnutritionPlansRepository.Delete(existingPlan);
                if (!deleted)
                    return new ApiResponse(null, "Error replacing diet plan", StatusCodes.Status500InternalServerError);
            }

            await _aiService.GetBasicFitnessPlan(userId);
            return new ApiResponse(userInfo, "User information and diet plan updated successfully");
        }

        public async Task<ApiResponse> CanUserUpdateDietPlan(string userId)
        {
            var user = await _userInformationRepository.GetByUserId(userId);
            if (user is null)
            {
                return new ApiResponse(null, "Invalid userid", StatusCodes.Status400BadRequest);
            }

            var fitnessPlansByUserId = await _fitnessAndnutritionPlansRepository.GetBasicPlanByUserIdList(userId);
            if (fitnessPlansByUserId?.Any() == true && fitnessPlansByUserId.Count >= 2)
            {
                return new ApiResponse(false, "User cannot update diet plan", StatusCodes.Status400BadRequest);
            }

            return new ApiResponse(true, "User can update diet plan", StatusCodes.Status400BadRequest);
        }

        private bool NeedToUpdateDietPlan(UpdateUserFitnessGoalReq updateUserFitnessGoalReq, UserInformation userInformation)
        {
            updateUserFitnessGoalReq.ActivityLevel = updateUserFitnessGoalReq.ActivityLevel ?? userInformation.ActivityLevel;
            updateUserFitnessGoalReq.Weight = updateUserFitnessGoalReq.Weight ?? userInformation.Weight;
            updateUserFitnessGoalReq.DietType = updateUserFitnessGoalReq.DietType ?? userInformation.DietType;
            updateUserFitnessGoalReq.FitnessGoal = updateUserFitnessGoalReq.FitnessGoal ?? userInformation.FitnessGoal;

            if (updateUserFitnessGoalReq.ActivityLevel != userInformation.ActivityLevel ||
            updateUserFitnessGoalReq.Weight != userInformation.Weight || updateUserFitnessGoalReq.FitnessGoal != userInformation.FitnessGoal)
            {
                return true;
            }

            return false;
        }

        private async Task<bool> HasUserExceededDietPlanLimit(string userId)
        {
            var fitnessPlansByUserId = await _fitnessAndnutritionPlansRepository.GetBasicPlanByUserIdList(userId ?? "");
            if (fitnessPlansByUserId?.Any() == true && fitnessPlansByUserId.Count >= 2)
            {
                return true;
            }

            return false;
        }
    }
}