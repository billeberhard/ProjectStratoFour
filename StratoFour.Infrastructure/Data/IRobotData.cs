using StratoFour.Infrastructure.Models;

namespace StratoFour.Infrastructure.Data
{
    public interface IRobotData
    {
        Task<IEnumerable<RobotModel>> GetAllRobots();
        Task<RobotModel> GetRobotById(int robotId);
        Task<RobotModel> GetReadyRobot();
        Task<int> InsertRobot(RobotModel robot);
        Task UpdateRobot(RobotModel robot);
        Task UpdateRobotStatus(int robotId, string robotStatus);
    }
}