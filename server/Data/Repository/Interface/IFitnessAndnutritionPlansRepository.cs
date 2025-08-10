

using Entity.DbModels;

namespace Data.Repository.Interface
{
    public interface IFitnessAndnutritionPlansRepository
    {
        Task<FitnessAndNutritionPlan?> Create(FitnessAndNutritionPlan fitnessAndNutritionPlan);
    }
}