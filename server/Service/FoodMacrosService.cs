using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repository.Interface;
using Entity.DbModels;
using Service.Helpers;
using Service.Interface;

namespace Service
{
    public class FoodMacrosService : IFoodMacrosService
    {
        private readonly IIndianFoodMacrosRepository _indianFoodMacrosRepository;

        public FoodMacrosService(IIndianFoodMacrosRepository indianFoodMacrosRepository)
        {
            _indianFoodMacrosRepository = indianFoodMacrosRepository;
        }

        public async Task<ApiResponse> GetFoodMacrosByName(string name)
        {
            return new ApiResponse(await _indianFoodMacrosRepository.GetFoodMacrosByName(name), "Food macros retrieved successfully.");
        }
    }
}