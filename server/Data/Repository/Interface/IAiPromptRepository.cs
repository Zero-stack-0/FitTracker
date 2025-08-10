using Entity.Models;
using static Entity.Enums;

namespace Data.Repository.Interface
{
    public interface IAiPromptRepository
    {
        Task<AiPrompt?> GetById(AiPromptType type);
        Task<AiResponse?> SaveAiResponse(AiResponse aiResponse);
        Task<string?> GetAiResponse();
    }
}