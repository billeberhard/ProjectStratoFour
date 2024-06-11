using StratoFour.Infrastructure.Data;
using StratoFour.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Application.UserMatching
{
    public class UserService : IUserService
    {
        private readonly IUserData _userData;
        public UserService(IUserData userData)
        {
            _userData = userData;
        }
        public Task AddUserAsync(UserModel user)
        {
            return _userData.InsertUser(user);
        }

        public Task DeleteUserAsync(int id)
        {
            return _userData.DeleteUser(id);
        }

        public Task<UserModel> GetUserByEmailAsync(string email)
        {
            return _userData.GetUserByEmail(email);
        }

        public Task<UserModel> GetUserByIdAsync(int id)
        {
            return _userData.GetUserById(id);
        }

        public Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            return _userData.GetUsers();
        }

        public Task UpdateUserAsync(UserModel user)
        {
            return _userData.UpdateUser(user);
        }

        public async Task UpdateUserConnectionIdAsync(int userId, string connectionId)
        {
            var user = await _userData.GetUserById(userId);
            if (user != null)
            {
                user.ConnectionId = connectionId;
                await _userData.UpdateUser(user);
            }
        }
    }
}
