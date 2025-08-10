using Data.Repository.Interface;
using Entity.DbModels;
using MongoDB.Driver;

namespace Data.Repository
{
    public class FitnessAndnutritionPlansRepository : IFitnessAndnutritionPlansRepository
    {
        private readonly IMongoCollection<FitnessAndNutritionPlan> _fitnessAndNutritionPlansCollection;
        public FitnessAndnutritionPlansRepository(FitTrackerDbContext context)
        {
            _fitnessAndNutritionPlansCollection = context.FitnessAndNutritionPlan;
        }

        public async Task<FitnessAndNutritionPlan?> Create(FitnessAndNutritionPlan fitnessAndNutritionPlan)
        {
            try
            {
                await _fitnessAndNutritionPlansCollection.InsertOneAsync(fitnessAndNutritionPlan);
                return fitnessAndNutritionPlan;
            }
            catch (Exception ex)
            {
                // Log the error if needed
                Console.WriteLine($"Insert failed: {ex.Message}");
                return null;
            }
        }
    }
}