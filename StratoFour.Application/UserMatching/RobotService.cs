using StratoFour.Infrastructure.Data;
using StratoFour.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Application.UserMatching
{
    public class RobotService : IRobotService
    {
        private readonly IRobotData _robotData;

        public RobotService(IRobotData robotData)
        {
            _robotData = robotData;
        }

        public Task<IEnumerable<RobotModel>> GetAllRobots() =>
            _robotData.GetAllRobots();

        public Task<RobotModel> GetRobotById(int robotId) =>
            _robotData.GetRobotById(robotId);

        public Task<RobotModel> GetReadyRobot() =>
            _robotData.GetReadyRobot();

        public Task<int> AddRobot(RobotModel robot) =>
            _robotData.InsertRobot(robot);

        public Task UpdateRobot(RobotModel robot) =>
            _robotData.UpdateRobot(robot);

        public Task UpdateRobotStatus(int robotId, string robotStatus) =>
        _robotData.UpdateRobotStatus(robotId, robotStatus);
    }
}
