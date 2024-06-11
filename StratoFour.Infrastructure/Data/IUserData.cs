using StratoFour.Infrastructure.Models;

namespace StratoFour.Infrastructure.Data
{
    public interface IUserData
    {
        Task DeleteUser(int id);
        Task<UserModel> GetUserById(int id);
        Task<UserModel> GetUserByEmail(string email);
        Task<IEnumerable<UserModel>> GetUsers();
        Task InsertUser(UserModel user);
        Task UpdateUser(UserModel user);
    }
}