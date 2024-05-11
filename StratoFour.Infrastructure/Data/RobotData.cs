using StratoFour.Infrastructure.DbAccess;
using StratoFour.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Infrastructure.Data;

public class RobotData : IRobotData
{
    private readonly ISqlDataAccess _db;

    public RobotData(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task<IEnumerable<RobotModel>> GetRobots() =>
        _db.LoadData<RobotModel, dynamic>("dbo.spUser_GetAll", new { });

    public async Task<RobotModel> GetRobots(int id)
    {
        var results = await _db.LoadData<RobotModel, dynamic>("dbo.spRobot_Get", new { Id = id });
        return results.FirstOrDefault();
    }

    public Task InsertRobots(RobotModel robot) => _db.SaveData("dbo.spRobot_Insert", new { robot.RobotName, robot.RobotStatus });


    public Task UpdateRobot(RobotModel robot) => _db.SaveData("dbo.spRobot_Update", robot);

    public Task DeleteRobot(int id) => _db.SaveData("dbo.spRobot_Delete", new { Id = id });
}
