using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repository.Interface;
using Data.response;
using Data.Response;
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

        public async Task<UserFoodLogWeekHistory> GetFoodLogEntriesByStartAndEndDate(string userId, DateTime startDate, DateTime endDate)
        {
            ObjectId obUserId = new ObjectId(userId);
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$match", new BsonDocument("userId", obUserId)),
                new BsonDocument("$match",
                new BsonDocument("createdAt",
                new BsonDocument
                {
                    { "$gte", startDate },
                    { "$lte", endDate }
                }
                            )
                        ),

                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "indian-food-macros" },
                    { "localField", "foodId" },
                    { "foreignField", "_id" },
                    { "as", "result" }
                }),

                new BsonDocument("$sort", new BsonDocument("createdAt", -1)),

                new BsonDocument("$project", new BsonDocument
                {
                    { "_id", 1 },
                    { "userId", "$userId" },
                    { "foodName", new BsonDocument("$arrayElemAt", new BsonArray { "$result.food", 0 }) },
                    { "caloriesLogged", "$calories" },
                    {"protein", "$protein"},
                    {"carb", "$carb"},
                    {"fat", "$fat"},
                    { "timeOfTheDay", "$timeOfTheDay" },
                    {"createdAt", "$createdAt"}
                })
            };

            var results = await _userFoodLogCollection
            .Aggregate<BsonDocument>(pipeline)
            .ToListAsync();

            var output = results.Select(doc => new UserFoodLogResponse
            {
                Id = doc.Contains("_id") ? doc["_id"].ToString() : "",
                UserId = doc.Contains("userId") ? doc["userId"].ToString() : null,
                FoodName = doc.Contains("foodName") ? doc["foodName"].AsString : null,
                CaloriesLogged = doc.GetValue("caloriesLogged", BsonNull.Value).IsBsonNull
                ? (double?)null
                : doc["caloriesLogged"].ToDouble(),
                ProtienLogged = doc.GetValue("protein", BsonNull.Value).IsBsonNull
                ? (double?)null
                : doc["protein"].ToDouble(),
                CarbLogged = doc.GetValue("carb", BsonNull.Value).IsBsonNull
                ? (double?)null
                : doc["carb"].ToDouble(),
                FatLogged = doc.GetValue("fat", BsonNull.Value).IsBsonNull
                ? (double?)null
                : doc["fat"].ToDouble(),

                TimeOfTheDay = doc.GetValue("timeOfTheDay", BsonNull.Value).IsBsonNull
                ? null
                : Enum.GetName(typeof(TimeOfTheDay), doc["timeOfTheDay"].ToInt32()),

                CreatedAt = doc.GetValue("createdAt", BsonNull.Value).IsBsonNull
                ? DateTime.UtcNow
                : doc["createdAt"].ToUniversalTime()
            }).ToList();

            var groupedFoodLogs = output
             .GroupBy(e => e.CreatedAt.Date)
             .Select(g => new UserFoodLogDayHistory
             {
                 CreatedDate = g.Key,
                 TotalCaloriesLogged = g.Sum(e => e.CaloriesLogged),
                 TotalCarbLogged = g.Sum(e => e.CarbLogged),
                 TotalFatLogged = g.Sum(e => e.FatLogged),
                 TotalProtienLogged = g.Sum(e => e.ProtienLogged),
                 FoodLogByTypeResponse = g.GroupBy(e => e.TimeOfTheDay)
                 .Select(r => new FoodLogByType
                 {
                     TimeOfTheDay = r.Key,
                     UserFoodLogResponseList = r.ToList()
                 }).ToList()
             })
             .ToList();

            var userFoodLogWeekHistory = new UserFoodLogWeekHistory
            {
                UserFoodLogDayHistory = groupedFoodLogs,
                StartDate = startDate,
                EndDate = endDate,
                TotalCaloriesLogged = groupedFoodLogs.Sum(e => e.TotalCaloriesLogged),
                TotalCarbLogged = groupedFoodLogs.Sum(e => e.TotalCarbLogged),
                TotalProtienLogged = groupedFoodLogs.Sum(e => e.TotalProtienLogged),
                TotalFatLogged = groupedFoodLogs.Sum(e => e.TotalFatLogged),
            };
            return userFoodLogWeekHistory;
        }

        public async Task<DashboardResponse> GetFoodLogEntriesForToday(string userId)
        {
            TimeZoneInfo indianZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            var nowInIndia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, indianZone);
            var today = nowInIndia.Date;
            var tomorrow = today.AddDays(1);
            var todayUtc = TimeZoneInfo.ConvertTimeToUtc(today, indianZone);
            var tomorrowUtc = TimeZoneInfo.ConvertTimeToUtc(tomorrow, indianZone);
            ObjectId obUserId = new ObjectId(userId);
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$match", new BsonDocument("userId", obUserId)),
                new BsonDocument("$match", new BsonDocument("createdAt", new BsonDocument
                {
                    { "$gte", today },
                    { "$lt", tomorrow }
                })),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "indian-food-macros" },
                    { "localField", "foodId" },
                    { "foreignField", "_id" },
                    { "as", "result" }
                }),

                new BsonDocument("$sort", new BsonDocument("createdAt", -1)),
                new BsonDocument("$project", new BsonDocument
                {
                    { "_id", 1 },
                    { "userId", "$userId" },
                    { "foodName", new BsonDocument("$arrayElemAt", new BsonArray { "$result.food", 0 }) },
                    { "caloriesLogged", "$calories" },
                    {"protein", "$protein"},
                    {"carb", "$carb"},
                    {"fat", "$fat"},
                    { "timeOfTheDay", "$timeOfTheDay" },
                    {"createdAt", "$createdAt"}
                })
            };

            var results = await _userFoodLogCollection
            .Aggregate<BsonDocument>(pipeline)
            .ToListAsync();

            var output = results.Select(doc => new UserFoodLogResponse
            {
                Id = doc.Contains("_id") ? doc["_id"].ToString() : "",
                UserId = doc.Contains("userId") ? doc["userId"].ToString() : null,
                FoodName = doc.Contains("foodName") ? doc["foodName"].AsString : null,
                CaloriesLogged = doc.GetValue("caloriesLogged", BsonNull.Value).IsBsonNull
                ? (double?)null
                : doc["caloriesLogged"].ToDouble(),
                ProtienLogged = doc.GetValue("protein", BsonNull.Value).IsBsonNull
                ? (double?)null
                : doc["protein"].ToDouble(),
                CarbLogged = doc.GetValue("carb", BsonNull.Value).IsBsonNull
                ? (double?)null
                : doc["carb"].ToDouble(),
                FatLogged = doc.GetValue("fat", BsonNull.Value).IsBsonNull
                ? (double?)null
                : doc["fat"].ToDouble(),

                TimeOfTheDay = doc.GetValue("timeOfTheDay", BsonNull.Value).IsBsonNull
                ? null
                : Enum.GetName(typeof(TimeOfTheDay), doc["timeOfTheDay"].ToInt32()),

                CreatedAt = doc.GetValue("createdAt", BsonNull.Value).IsBsonNull
                ? DateTime.UtcNow
                : doc["createdAt"].ToUniversalTime()
            }).ToList();

            return new DashboardResponse
            {
                TotalCaloriesLogged = output.Sum(e => e.CaloriesLogged),
                TotalCarbLogged = output.Sum(e => e.CarbLogged),
                TotalFatLogged = output.Sum(e => e.FatLogged),
                TotalProtienLogged = output.Sum(e => e.ProtienLogged)
            };
        }
    }
}