using StratoFour.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Application.UserMatching;

public interface IRobotService
{
    Task<IEnumerable<RobotModel>> GetAllRobots();
    Task<RobotModel> GetRobotById(int robotId);
    Task<RobotModel> GetReadyRobot();
    Task<int> AddRobot(RobotModel robot);
    Task UpdateRobot(RobotModel robot);
    Task UpdateRobotStatus(int robotId, string robotStatus);
}
