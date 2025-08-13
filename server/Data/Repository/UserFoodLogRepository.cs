using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repository.Interface;
using Entity.DbModels;
using MongoDB.Bson;
using MongoDB.Driver;
using static Entity.Enums;

namespace Data.Repository
{
    public class UserFoodLogRepository : IUserFoodLogRepository
    {
        private readonly IMongoCollection<UserFoodLog> _userFoodLogCollection;
        public UserFoodLogRepository(FitTrackerDbContext context)
        {
            _userFoodLogCollection = context.UserFoodLog;
        }

        public async Task<UserFoodLog?> Create(UserFoodLog userFoodLog)
        {
            return await _userFoodLogCollection.InsertOneAsync(userFoodLog).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    return null;
                }
                return userFoodLog;
            });
        }

        public async Task<List<object>> GetRecentFoodLogEntriesAsync(string userId)
        {
            ObjectId obUserId = new ObjectId(userId);
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$match", new BsonDocument("userId", obUserId)),

                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "indian-food-macros" },
                    { "localField", "foodId" },
                    { "foreignField", "_id" },
                    { "as", "result" }
                }),

                new BsonDocument("$sort", new BsonDocument("createdAt", -1)),

                new BsonDocument("$limit", 3),

                new BsonDocument("$project", new BsonDocument
                {
                    { "_id", 1 },
                    { "userId", "$userId" },
                    { "foodName", new BsonDocument("$arrayElemAt", new BsonArray { "$result.food", 0 }) },
                    { "caloriesLogged", "$calories" },
                    { "timeOfTheDay", "$timeOfTheDay" }
                })
            };

            var results = await _userFoodLogCollection
            .Aggregate<BsonDocument>(pipeline)
            .ToListAsync();

            var output = results.Select(doc => new
            {
                _id = doc.Contains("_id") ? doc["_id"].ToString() : null,
                userId = doc.Contains("userId") ? doc["userId"].ToString() : null,
                foodName = doc.Contains("foodName") ? doc["foodName"].AsString : null,
                caloriesLogged = doc.GetValue("caloriesLogged", BsonNull.Value).IsBsonNull
                ? (double?)null
                : doc["caloriesLogged"].ToDouble(),

                timeOfTheDay = doc.GetValue("timeOfTheDay", BsonNull.Value).IsBsonNull
        ? null
        : Enum.GetName(typeof(TimeOfTheDay), doc["timeOfTheDay"].ToInt32())
            }).Cast<dynamic>().ToList();


            return output;
        }


    }
}