using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repository.Interface;
using Entity.DbModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Service.Dto.Request;
using Service.Helpers;
using Service.Interface;
using static Entity.Enums;

namespace Service
{
    public class UserFoodLogService : IUserFoodLogService
    {
        private readonly IUserFoodLogRepository _userFoodLogRepository;
        private readonly IIndianFoodMacrosRepository _indianFoodMacrosRepository;
        public UserFoodLogService(IUserFoodLogRepository userFoodLogRepository, IIndianFoodMacrosRepository indianFoodMacrosRepository)
        {
            _userFoodLogRepository = userFoodLogRepository;
            _indianFoodMacrosRepository = indianFoodMacrosRepository;
        }

        public async Task<ApiResponse> Create(CreateUserFoodLogRq req)
        {
            if (string.IsNullOrWhiteSpace(req.UserId))
            {
                return new ApiResponse(null, "Invalid requestor id", StatusCodes.Status403Forbidden);
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
        public CalculatedMaros CalculateMacros(IndianFoodMacros macrosPer100g, double gramsInput)
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
    }
}