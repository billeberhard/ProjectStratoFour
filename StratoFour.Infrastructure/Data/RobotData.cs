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

    public Task<IEnumerable<RobotModel>> GetAllRobots() =>
        _db.LoadData<RobotModel, dynamic>("dbo.spRobot_GetAll", new { });

    public async Task<RobotModel> GetRobotById(int robotId)
    {
        var results = await _db.LoadData<RobotModel, dynamic>("dbo.spRobot_GetById", new { RobotId = robotId });
        return results.FirstOrDefault();
    }

    public async Task<RobotModel> GetReadyRobot()
    {
        var results = await _db.LoadData<RobotModel, dynamic>("dbo.spRobot_GetReady", new { });
        return results.FirstOrDefault();
    }

    public async Task<int> InsertRobot(RobotModel robot)
    {
        var result = await _db.LoadData<int, dynamic>("dbo.spRobot_Insert", new { robot.RobotName, robot.RobotStatus });
        return result.FirstOrDefault();
    }

    public Task UpdateRobot(RobotModel robot) =>
        _db.SaveData("dbo.spRobot_Update", new { robot.RobotId, robot.RobotName, robot.RobotStatus });

    public Task UpdateRobotStatus(int robotId, string robotStatus) =>
    _db.SaveData("dbo.spRobot_UpdateStatus", new { RobotId = robotId, RobotStatus = robotStatus });

}
