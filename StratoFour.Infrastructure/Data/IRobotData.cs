using StratoFour.Infrastructure.Models;

namespace StratoFour.Infrastructure.Data
{
    public interface IRobotData
    {
        Task DeleteUser(int id);
        Task<IEnumerable<RobotModel>> GetRobots();
        Task<RobotModel> GetRobots(int id);
        Task InsertRobots(RobotModel robot);
        Task UpdateUser(RobotModel robot);
    }
}