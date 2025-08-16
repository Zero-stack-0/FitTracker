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

        public async Task<Users?> GetById(string id)
        {
            return await usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
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
            new BsonDocument("$unwind", "$basic"),
            new BsonDocument("$match",
            new BsonDocument("basic.isActive", true)),
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
                    { "macroTargets", "$basic.macroTargets" }
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

        public async Task<bool> Update(Users user)
        {
            var filter = Builders<Users>.Filter.Eq(p => p.Id, user.Id);
            var update = Builders<Users>.Update
                .Set(p => p.FullName, user.FullName)
                .Set(p => p.UpdatedAt, DateTime.UtcNow);

            var result = await usersCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }
    }
}