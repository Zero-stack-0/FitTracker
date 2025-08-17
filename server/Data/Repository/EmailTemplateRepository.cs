using Data.Repository.Interface;
using Entity.DbModels;
using MongoDB.Driver;
using static Entity.Enums;

namespace Data.Repository
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        private readonly IMongoCollection<EmailTemplate> _emailTemplateCollection;
        public EmailTemplateRepository(FitTrackerDbContext context)
        {
            _emailTemplateCollection = context.EmailTemplate;
        }

        public async Task<EmailTemplate?> GetByType(EMAIL_TEMPLATE_TYPE type)
        {
            return await _emailTemplateCollection.Find(e => e.Type == type).FirstOrDefaultAsync();
        }
    }
}