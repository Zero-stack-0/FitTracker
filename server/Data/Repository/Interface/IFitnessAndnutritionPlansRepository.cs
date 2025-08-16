

using Entity.DbModels;
using Entity.Models;

namespace Data.Repository.Interface
{
    public interface IFitnessAndnutritionPlansRepository
    {
        Task<FitnessAndNutritionPlan?> Create(FitnessAndNutritionPlan fitnessAndNutritionPlan);
        Task<BasicFitnessPlan?> Create(BasicFitnessPlan basicFitnessPlan);
        Task<BasicFitnessPlan?> GetBasicPlanByUserId(string userId);
        Task<List<BasicFitnessPlan>?> GetBasicPlanByUserIdList(string userId);
        Task<bool> Delete(BasicFitnessPlan req);
    }
}