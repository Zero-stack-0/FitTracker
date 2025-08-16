using Data.Repository.Interface;
using Entity.DbModels;
using MongoDB.Driver;

namespace Data.Repository
{
    public class MotivationRepository : IMotivationRepository
    {
        private readonly IMongoCollection<Motivation> _motivationCollection;
        public MotivationRepository(FitTrackerDbContext context)
        {
            _motivationCollection = context.Motivation;
        }

        public async Task<Motivation?> GetByNumber(int number)
        {
            return await _motivationCollection.Find(e => e.Number == number).FirstOrDefaultAsync();
        }
    }
}