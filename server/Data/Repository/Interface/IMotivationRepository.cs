using Entity.DbModels;

namespace Data.Repository.Interface
{
    public interface IMotivationRepository
    {
        Task<Motivation?> GetByNumber(int number);
    }
}