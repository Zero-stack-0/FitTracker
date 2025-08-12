using Data.Repository.Interface;
using Entity.DbModels;
using MongoDB.Driver;

namespace Data.Repository
{
    public class IndianFoodMacrosRepository : IIndianFoodMacrosRepository
    {
        private readonly IMongoCollection<IndianFoodMacros> _foodMacrosCollection;
        public IndianFoodMacrosRepository(FitTrackerDbContext context)
        {
            _foodMacrosCollection = context.FoodMacros;
        }

        public async Task<List<IndianFoodMacros>> GetFoodMacrosByName(string name)
        {
            // Case-insensitive search
            var filter = Builders<IndianFoodMacros>.Filter.Regex(f => f.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));

            var allMatches = await _foodMacrosCollection.Find(filter).ToListAsync();

            // Distinct by Food name (case-insensitive)
            var distinctFoods = allMatches
                .GroupBy(f => f.Name.ToLower())
                .Select(g => g.First())
                .ToList();

            // Ordering: exact match → starts with → contains
            var orderedFoods = distinctFoods
                .OrderByDescending(f => f.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
                .ThenByDescending(f => f.Name.StartsWith(name, System.StringComparison.OrdinalIgnoreCase))
                .ThenByDescending(f => f.Name.Contains(name, System.StringComparison.OrdinalIgnoreCase))
                .ToList();

            return orderedFoods;
        }
    }
}