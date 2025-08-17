using Entity.DbModels;
using static Entity.Enums;

namespace Data.Repository.Interface
{
    public interface IEmailTemplateRepository
    {
        Task<EmailTemplate?> GetByType(EMAIL_TEMPLATE_TYPE type);
    }
}