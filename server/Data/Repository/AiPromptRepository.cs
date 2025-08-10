using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repository.Interface;
using Entity.Models;
using MongoDB.Driver;
using static Entity.Enums;

namespace Data.Repository
{
    public class AiPromptRepository : IAiPromptRepository
    {
        private readonly IMongoCollection<AiPrompt> _aiPromptCollection;
        private readonly IMongoCollection<AiResponse> _aiResponseCollection;
        public AiPromptRepository(FitTrackerDbContext context)
        {
            _aiPromptCollection = context.AiPrompt;
            _aiResponseCollection = context.AiResponse;
        }

        public async Task<AiResponse?> SaveAiResponse(AiResponse aiResponse)
        {
            return await _aiResponseCollection.InsertOneAsync(aiResponse).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    return null;
                }
                return aiResponse;
            });
        }

        public async Task<string?> GetAiResponse()
        {
            var response = await _aiResponseCollection.Find(_ => true).FirstOrDefaultAsync();
            return response?.Response;
        }

        public async Task<AiPrompt?> GetById(AiPromptType type)
        {
            return await _aiPromptCollection.Find(ap => ap.Type == type).FirstOrDefaultAsync();
        }
    }
}