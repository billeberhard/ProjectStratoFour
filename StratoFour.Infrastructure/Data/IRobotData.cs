using StratoFour.Infrastructure.Models;

namespace StratoFour.Infrastructure.Data
{
    public interface IRobotData
    {
        Task DeleteRobot(int id);
        Task<IEnumerable<RobotModel>> GetRobots();
        Task<RobotModel> GetRobots(int id);
        Task InsertRobots(RobotModel robot);
        Task UpdateRobot(RobotModel robot);
    }
}