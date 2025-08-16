using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repository.Interface;
using Service.Helpers;
using Service.Interface;

namespace Service
{
    public class MotivationService : IMotivationService
    {
        private readonly IMotivationRepository _motivationRepository;
        public MotivationService(IMotivationRepository motivationRepository)
        {
            _motivationRepository = motivationRepository;
        }

        public async Task<ApiResponse> GetMotivation()
        {
            var random = new Random();
            return new ApiResponse(await _motivationRepository.GetByNumber(random.Next(1, 51)), "Motivation");
        }
    }
}