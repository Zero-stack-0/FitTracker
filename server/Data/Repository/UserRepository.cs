using Data.Repository.Interface;
using Data.response;
using Entity.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<Users> usersCollection;
        public UserRepository(FitTrackerDbContext context)
        {
            this.usersCollection = context.Users;
        }

        public async Task<Users?> Create(Users user)
        {
            return await usersCollection.InsertOneAsync(user).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    return null;
                }
                return user;
            });
        }

        public async Task<Users?> GetByEmail(string email)
        {
            return await usersCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Users?> Login(string email, string password)
        {
            return await usersCollection.Find(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();
        }

        public async Task<UserInfomationResponse> GetUserInformation(string email)
        {
            var pipeline = new BsonDocument[]
            {

                new BsonDocument("$match",
                new BsonDocument("email", email)),
                new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "UserInformation" },
                    { "localField", "_id" },
                    { "foreignField", "userId" },
                    { "as", "result" }
                }),
                new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "basic-fitness-plan" },
                    { "localField", "_id" },
                    { "foreignField", "userId" },
                    { "as", "basic" }
                }),
                new BsonDocument("$project",
                new BsonDocument
                {
                    { "_id", 1 },
                    { "fullName", 1 },
                    {"email", 1},
                    { "userInformation",
                new BsonDocument("$arrayElemAt",
                new BsonArray
                        {
                            "$result",
                            0
                        }) },
                    { "macroTargets",
                new BsonDocument("$arrayElemAt",
                new BsonArray
                        {
                            "$basic.macroTargets",
                            0
                        }) }
                })

            };

            var results = await usersCollection
            .Aggregate<BsonDocument>(pipeline)
            .FirstOrDefaultAsync();

            UserInfomationResponse response = results != null
            ? BsonSerializer.Deserialize<UserInfomationResponse>(results)
            : null;

            return response ?? new UserInfomationResponse();
        }
    }
}