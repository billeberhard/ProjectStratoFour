using StratoFour.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Application.UserMatching;

public interface IUserService
{
    Task<IEnumerable<UserModel>> GetUsersAsync();
    Task<UserModel> GetUserByIdAsync(int id);
    Task<UserModel> GetUserByEmailAsync(string email);
    Task AddUserAsync(UserModel user);
    Task UpdateUserAsync(UserModel user);
    Task DeleteUserAsync(int id);
    Task UpdateUserConnectionIdAsync(int userId, string connectionId);
}
