using Entity.DbModels;
using static Entity.Enums;

namespace Data.Repository.Interface
{
    public interface IEmailLoggerRepository
    {
        Task<EmailLogger?> Create(EmailLogger emailLogger);
        Task<List<EmailLogger>> GetByUserIdAndType(string userId, EMAIL_TEMPLATE_TYPE type);
    }
}