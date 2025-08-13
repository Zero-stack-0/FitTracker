using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repository.Interface;
using Entity.DbModels;
using MongoDB.Driver;

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
    }
}