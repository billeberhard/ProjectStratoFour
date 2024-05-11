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

        public Task<IEnumerable<GameModel>> GetGame() =>
            _db.LoadData<GameModel, dynamic>("dbo.spGame_GetAll", new { });

        public async Task<GameModel> GetGame(int id)
        {
            var results = await _db.LoadData<GameModel, dynamic>("dbo.spGame_Get", new { Id = id });
            return results.FirstOrDefault();
        }

        public Task InsertGame(GameModel game) => _db.SaveData("dbo.spGame_Insert", new { game.Player1Id, game.Player2Id, game.RobotId });


        public Task UpdateGame(GameModel game) => _db.SaveData("dbo.spGame_Update", game);

        public Task DeleteGame(int id) => _db.SaveData("dbo.spGame_Delete", new { Id = id });
    }
}
