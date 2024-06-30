using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace StratoFour.RestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private static readonly ConcurrentDictionary<int, int> GameMoves = new ConcurrentDictionary<int, int>();

        [HttpPost("move")]
        public IActionResult PostMove([FromQuery] int gameId, [FromQuery] int column)
        {
            GameMoves[gameId] = column;
            return Ok();
        }

        [HttpGet("nextmove")]
        public IActionResult GetNextMove([FromQuery] int gameId)
        {
            if (GameMoves.TryGetValue(gameId, out int column))
            {
                return Ok(column);
            }
            return NotFound();
        }

        //[HttpGet("{gameId}/status")]
        //public IActionResult GetGameStatus(int gameId)
        //{
        //    var game = _gameService.GetGameById(gameId);
        //    return Ok(new { game.GetBoard(), game.IsOver(), game.GetCurrentPlayer(), game.GetWinner() });
        //}

        //[HttpPost("confirm")]
        //public IActionResult ConfirmGame([FromBody] bool confirmGame)
        //{
        //    _gameService.ConfirmGame(request.GameId);
        //    return Ok();
        //}
    }
}
