using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repository.Interface;
using Entity.DbModels;
using MongoDB.Driver;
using static Entity.Enums;

namespace Data.Repository
{
    public class EmailLoggerRepository : IEmailLoggerRepository
    {
        private readonly IMongoCollection<EmailLogger> _emailLoggerCollection;
        public EmailLoggerRepository(FitTrackerDbContext context)
        {
            _emailLoggerCollection = context.EmailLogger;
        }

        public async Task<EmailLogger?> Create(EmailLogger emailLogger)
        {
            return await _emailLoggerCollection.InsertOneAsync(emailLogger).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    return null;
                }
                return emailLogger;
            });
        }

        public async Task<List<EmailLogger>> GetByUserIdAndType(string userId, EMAIL_TEMPLATE_TYPE type)
        {
            return await _emailLoggerCollection.Find(e => e.UserId == userId && e.EmailTemplateType == type && e.IsEmailSent).ToListAsync();
        }
    }
}