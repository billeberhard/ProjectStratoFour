using StratoFour.Infrastructure.DbAccess;
using StratoFour.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Infrastructure.Data
{
    public class GameData : IGameData
    {
        private readonly ISqlDataAccess _db;

        public GameData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<GameModel>> GetGames() =>
            _db.LoadData<GameModel, dynamic>("dbo.spGame_GetAll", new { });

        public async Task<GameModel> GetGame(int id)
        {
            var results = await _db.LoadData<GameModel, dynamic>("dbo.spGame_Get", new { Id = id });
            return results.FirstOrDefault();
        }

        public async Task<int> CreateGame(GameModel game)
        {
            var parameters = new
            {
                game.Player1Id,
                game.Player2Id,
                game.RobotId,
                StartTime = game.StartTime,
                IsActive = game.IsActive
            };
            var result = await _db.LoadData<int, dynamic>("dbo.spGame_Insert", parameters);
            return result.FirstOrDefault();
        }

        public Task UpdateGame(GameModel game) =>
            _db.SaveData("dbo.spGame_Update", new { game.SessionId, game.WinnerId, game.IsActive });

        public Task DeleteGame(int id) =>
            _db.SaveData("dbo.spGame_Delete", new { Id = id });

        public async Task<GameModel> GetActivGameSession(int userId)
        {
            var results = await _db.LoadData<GameModel, dynamic>("dbo.spGame_GetActive", new { UserId = userId });
            return results.FirstOrDefault();
        }
    }
}
