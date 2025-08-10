using Entity.Models;

namespace Data.Repository.Interface
{
    public interface IUserRepository
    {
        Task<Users?> Create(Users user);
        Task<Users?> GetByEmail(string email);
        Task<Users?> Login(string email, string password);
    }
}