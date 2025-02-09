﻿using StratoFour.Infrastructure.DbAccess;
using StratoFour.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Infrastructure.Data;

public class UserData : IUserData
{
    private readonly ISqlDataAccess _db;

    public UserData(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task<IEnumerable<UserModel>> GetUsers() =>
        _db.LoadData<UserModel, dynamic>("dbo.spUser_GetAll", new { });

    public async Task<UserModel> GetUserById(int id)
    {
        var results = await _db.LoadData<UserModel, dynamic>("dbo.spUser_Get", new { UserId = id });
        return results.FirstOrDefault();
    }

    public async Task<UserModel> GetUserByEmail(string email)
    {
        var results = await _db.LoadData<UserModel, dynamic>("dbo.spUser_GetByEmail", new { Email = email });
        return results.FirstOrDefault();
    }

    public Task InsertUser(UserModel user) => _db.SaveData("dbo.spUser_Insert", new { user.Username, user.Email, user.PasswordHash });


    public Task UpdateUser(UserModel user) => _db.SaveData("dbo.spUser_Update", user);

    public Task DeleteUser(int id) => _db.SaveData("dbo.spUser_Delete", new { UserId = id });
}
