using StratoFour.Infrastructure.Models;

namespace StratoFour.Infrastructure
{
    public interface IUserData
    {
        Task<List<UserModel>> GetUsers();
        Task InsertUser(UserModel user);
    }
}