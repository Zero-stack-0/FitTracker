using Data.Repository.Interface;
using Entity.DbModels;
using Entity.Models;
using MongoDB.Driver;

namespace Data.Repository
{
    public class FitnessAndnutritionPlansRepository : IFitnessAndnutritionPlansRepository
    {
        private readonly IMongoCollection<FitnessAndNutritionPlan> _fitnessAndNutritionPlansCollection;
        private readonly IMongoCollection<BasicFitnessPlan> _basicFitnessPlanCollection;
        public FitnessAndnutritionPlansRepository(FitTrackerDbContext context)
        {
            _fitnessAndNutritionPlansCollection = context.FitnessAndNutritionPlan;
            _basicFitnessPlanCollection = context.BasicFitnessPlan;
        }

        public async Task<FitnessAndNutritionPlan?> Create(FitnessAndNutritionPlan fitnessAndNutritionPlan)
        {
            await _fitnessAndNutritionPlansCollection.InsertOneAsync(fitnessAndNutritionPlan);
            return fitnessAndNutritionPlan;
        }

        public async Task<BasicFitnessPlan?> Create(BasicFitnessPlan basicFitnessPlan)
        {
            await _basicFitnessPlanCollection.InsertOneAsync(basicFitnessPlan);
            return basicFitnessPlan;
        }

        public async Task<BasicFitnessPlan?> GetBasicPlanByUserId(string userId)
        {
            return await _basicFitnessPlanCollection.Find(e => e.UserId == userId && e.IsActive).FirstOrDefaultAsync();
        }

        public async Task<List<BasicFitnessPlan>?> GetBasicPlanByUserIdList(string userId)
        {
            return await _basicFitnessPlanCollection.Find(e => e.UserId == userId && e.CreatedAt.Month == DateTime.UtcNow.Month).ToListAsync();
        }

        public async Task<bool> Delete(BasicFitnessPlan req)
        {
            var filter = Builders<BasicFitnessPlan>.Filter.Eq(p => p.UserId, req.UserId);
            var update = Builders<BasicFitnessPlan>.Update
                .Set(p => p.IsActive, false)
                .Set(p => p.UpdatedAt, DateTime.UtcNow);

            var result = await _basicFitnessPlanCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }
    }
}