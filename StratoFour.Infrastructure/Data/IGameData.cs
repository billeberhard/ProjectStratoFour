using StratoFour.Infrastructure.Models;

namespace StratoFour.Infrastructure.Data
{
    public interface IGameData
    {
        Task DeleteGame(int id);
        Task<IEnumerable<GameModel>> GetGame();
        Task<GameModel> GetGame(int id);
        Task InsertGame(GameModel game);
        Task UpdateGame(GameModel game);
    }
}