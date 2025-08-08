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
    }
}