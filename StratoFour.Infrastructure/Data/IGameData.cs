using StratoFour.Infrastructure.Models;

namespace StratoFour.Infrastructure.Data
{
    public interface IGameData
    {
        Task<int> CreateGame(GameModel game);
        Task DeleteGame(int id);
        Task<GameModel> GetActivGameSession(int userId);
        Task<GameModel> GetGame(int id);
        Task<IEnumerable<GameModel>> GetGames();
        Task UpdateGame(GameModel game);
    }
}