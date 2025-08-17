using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repository.Interface;
using Entity.DbModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Service.Dto.Request;
using Service.Dto.Response;
using Service.Helpers;
using Service.Interface;
using static Entity.Enums;

namespace Service
{
    public class UserFoodLogService : IUserFoodLogService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserFoodLogRepository _userFoodLogRepository;
        private readonly IIndianFoodMacrosRepository _indianFoodMacrosRepository;
        private readonly IFitnessAndnutritionPlansRepository _fitnessAndnutritionPlansRepository;
        public UserFoodLogService(IUserRepository userRepository, IFitnessAndnutritionPlansRepository fitnessAndnutritionPlansRepository, IUserFoodLogRepository userFoodLogRepository, IIndianFoodMacrosRepository indianFoodMacrosRepository)
        {
            _userRepository = userRepository;
            _userFoodLogRepository = userFoodLogRepository;
            _indianFoodMacrosRepository = indianFoodMacrosRepository;
            _fitnessAndnutritionPlansRepository = fitnessAndnutritionPlansRepository;
        }

        public async Task<ApiResponse> Create(CreateUserFoodLogRq req)
        {
            if (string.IsNullOrWhiteSpace(req.UserId))
            {
                return new ApiResponse(null, "Invalid requestor id", StatusCodes.Status403Forbidden);
            }

            var user = await _userRepository.GetById(req.UserId);
            if (user is null)
            {
                return new ApiResponse(null, "Invalid requestor id", StatusCodes.Status403Forbidden);
            }

            if (!user.IsEmailVerified)
            {
                return new ApiResponse(null, "Please verify your email", StatusCodes.Status400BadRequest);
            }

            var foodToLog = await _indianFoodMacrosRepository.GetById(req.FoodId);
            if (foodToLog is null)
            {
                return new ApiResponse(null, "Invalid food id", StatusCodes.Status400BadRequest);
            }

            if (req.Quantity <= 0)
            {
                return new ApiResponse(null, "Please enter valid quantity of food", StatusCodes.Status400BadRequest);
            }

            var calculatedMaros = CalculateMacros(foodToLog, req.Quantity);
            if (calculatedMaros is null)
            {
                return new ApiResponse(null, "Invalid calculation for macros", StatusCodes.Status400BadRequest);
            }

            var userFoodLog = new UserFoodLog
            {
                UserId = req.UserId,
                FoodId = foodToLog.Id,
                Quantity = req.Quantity,
                TimeOfTheDay = req.TimeOfTheDay,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                UpdatedAt = null,
                ProteinG = calculatedMaros.ProteinG,
                FatG = calculatedMaros.FatG,
                Calories = calculatedMaros.Calories,
                CarbsG = calculatedMaros.CarbsG
            };

            var addedFood = await _userFoodLogRepository.Create(userFoodLog);
            if (addedFood is null)
            {
                return new ApiResponse(null, "Something went wrong while adding food log", StatusCodes.Status500InternalServerError);
            }

            return new ApiResponse(userFoodLog, "User Food added successfully", StatusCodes.Status201Created);
        }

        public async Task<ApiResponse> GetRecentFoodLogEntriesAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new ApiResponse(null, "Invalid requestor id", StatusCodes.Status403Forbidden);
            }
            var user = await _userRepository.GetById(userId);
            if (user is null)
            {
                return new ApiResponse(null, "Invalid requestor id", StatusCodes.Status403Forbidden);
            }

            if (!user.IsEmailVerified)
            {
                return new ApiResponse(null, "Please verify your email", StatusCodes.Status400BadRequest);
            }
            return new ApiResponse(await _userFoodLogRepository.GetRecentFoodLogEntriesAsync(userId), "Recent fool log entries");
        }
        private CalculatedMaros CalculateMacros(IndianFoodMacros macrosPer100g, double gramsInput)
        {
            double factor = gramsInput / macrosPer100g.ServingSizeG;

            return new CalculatedMaros
            {
                Calories = Math.Round(macrosPer100g.Calories * factor, 2),
                ProteinG = Math.Round(macrosPer100g.ProteinG * factor, 2),
                CarbsG = Math.Round(macrosPer100g.CarbsG * factor, 2),
                FatG = Math.Round(macrosPer100g.FatG * factor, 2)
            };
        }

        public async Task<ApiResponse> GetFoodLogHistory(string userId, int weekOffset)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new ApiResponse(null, "Invalid requestor id", StatusCodes.Status403Forbidden);
            }

            var user = await _userRepository.GetById(userId);
            if (user is null)
            {
                return new ApiResponse(null, "Invalid requestor id", StatusCodes.Status403Forbidden);
            }

            if (!user.IsEmailVerified)
            {
                return new ApiResponse(null, "Please verify your email", StatusCodes.Status400BadRequest);
            }

            var (start, end) = GetWeekRange(weekOffset);

            var foodHistoryByWeek = await _userFoodLogRepository.GetFoodLogEntriesByStartAndEndDate(userId, start, end);

            return new ApiResponse(foodHistoryByWeek, "Food log history");
        }

        public async Task<ApiResponse> GetDashBoardResponseForUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new ApiResponse(null, "Invalid requestor id", StatusCodes.Status403Forbidden);
            }

            var user = await _userRepository.GetById(userId);
            if (user is null)
            {
                return new ApiResponse(null, "Invalid requestor id", StatusCodes.Status403Forbidden);
            }

            if (!user.IsEmailVerified)
            {
                return new ApiResponse(null, "Please verify your email", StatusCodes.Status400BadRequest);
            }

            var dashboard = await _userFoodLogRepository.GetFoodLogEntriesForToday(userId);

            var macrosTargets = await _fitnessAndnutritionPlansRepository.GetBasicPlanByUserId(userId);

            dashboard.MacroTargets = macrosTargets?.MacroTargets;

            return new ApiResponse(dashboard, "Dashbaord");
        }

        private static (DateTime WeekStart, DateTime WeekEnd) GetWeekRange(int offset)
        {
            DateTime today = DateTime.UtcNow.Date;

            int daysSinceMonday = (int)today.DayOfWeek - (int)DayOfWeek.Monday;
            if (daysSinceMonday < 0)
                daysSinceMonday += 7;

            DateTime weekStart = today.AddDays(-daysSinceMonday);
            weekStart = weekStart.AddDays(offset * 7);

            DateTime weekEnd = weekStart.AddDays(6).AddDays(1).AddTicks(-1);

            return (weekStart, weekEnd);
        }
    }
}