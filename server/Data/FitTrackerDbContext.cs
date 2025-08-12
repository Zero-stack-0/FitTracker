using Entity.DbModels;
using Entity.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Data
{
    public class FitTrackerDbContext
    {
        private readonly IMongoDatabase database;
        private readonly IOptions<MongoDbMappingConfiguration> config;

        public FitTrackerDbContext(IOptions<MongoDbMappingConfiguration> config)
        {
            this.config = config;

            var client = new MongoClient(this.config.Value.ConnectionString);
            database = client.GetDatabase(this.config.Value.DatabaseName);
        }

        public IMongoCollection<Users> Users =>
            database.GetCollection<Users>(
                (config.Value.Collections != null ? config.Value.Collections.FirstOrDefault(c => c == "users") : null) ?? "users"
            );

        public IMongoCollection<UserInformation> UserInformation =>
            database.GetCollection<UserInformation>(
                (config.Value.Collections != null ? config.Value.Collections.FirstOrDefault(c => c == "UserInformation") : null) ?? "UserInformation"
            );

        public IMongoCollection<AiPrompt> AiPrompt =>
            database.GetCollection<AiPrompt>(
                (config.Value.Collections != null ? config.Value.Collections.FirstOrDefault(c => c == "ai-prompts") : null) ?? "ai-prompts"
            );

        public IMongoCollection<AiResponse> AiResponse =>
            database.GetCollection<AiResponse>(
                (config.Value.Collections != null ? config.Value.Collections.FirstOrDefault(c => c == "AiResponse") : null) ?? "AiResponse"
            );

        public IMongoCollection<FitnessAndNutritionPlan> FitnessAndNutritionPlan =>
            database.GetCollection<FitnessAndNutritionPlan>(
                (config.Value.Collections != null ? config.Value.Collections.FirstOrDefault(c => c == "FitnessAndNutritionPlan") : null) ?? "FitnessAndNutritionPlan"
            );

        public IMongoCollection<IndianFoodMacros> FoodMacros =>
            database.GetCollection<IndianFoodMacros>(
                (config.Value.Collections != null ? config.Value.Collections.FirstOrDefault(c => c == "indian-food-macros") : null) ?? "indian-food-macros"
            );
    }
}