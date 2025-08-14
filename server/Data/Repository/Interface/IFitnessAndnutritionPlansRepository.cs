

using Entity.DbModels;
using Entity.Models;

namespace Data.Repository.Interface
{
    public interface IFitnessAndnutritionPlansRepository
    {
        Task<FitnessAndNutritionPlan?> Create(FitnessAndNutritionPlan fitnessAndNutritionPlan);
        Task<BasicFitnessPlan?> Create(BasicFitnessPlan basicFitnessPlan);
        Task<BasicFitnessPlan?> GetBasicPlanByUserId(string userId);
    }
}