using StratoFour.Infrastructure.Data;
using StratoFour.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Application.UserMatching
{   
    public class GameService : IGameService
    {
        private readonly IGameData _gameData;

        public GameService(IGameData gameData)
        {
            _gameData = gameData;
        }

        public async Task<int> CreateGameAsync(int player1Id, int player2Id, int robotId)
        {
            var game = new GameModel
            {
                Player1Id = player1Id,
                Player2Id = player2Id,
                RobotId = robotId,
                StartTime = DateTime.Now,
                IsActive = true
            };
            return await _gameData.CreateGame(game);
        }

        public async Task DeleteGameAsync(int id)
        {
            await _gameData.DeleteGame(id);
        }

        public async Task<GameModel> GetActiveGameSessionAsync(int userId)
        {
            return await _gameData.GetActivGameSession(userId);
        }

        public async Task<GameModel> GetGameAsync(int id)
        {
            return await _gameData.GetGame(id);
        }

        public async Task UpdateGameAsync(GameModel game)
        {
            await _gameData.UpdateGame(game);
        }
    }
}
