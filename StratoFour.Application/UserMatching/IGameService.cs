using StratoFour.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Application.UserMatching;

public interface IGameService
{
    Task<int> CreateGameAsync(int player1Id, int player2Id, int robotId);
    Task<GameModel> GetActiveGameSessionAsync(int userId);
    Task<GameModel> GetGameByIdAsync(int id);
    Task UpdateGameAsync(GameModel game);
    Task DeleteGameAsync(int id);
    void ConfirmGame(int gameId);
}
