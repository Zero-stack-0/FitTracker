using Data.Repository.Interface;
using Entity.Models;
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
    }
}