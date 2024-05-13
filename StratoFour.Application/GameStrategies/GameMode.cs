using StratoFour.Application.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Application.GameStrategies
{
    class GameModeFactory
    {
        public static IGameMode Create(GameModeLevel level, IGameBoard board)
        {
            return level switch
            {
                GameModeLevel.Easy => new EasyLevelStrategy(board),
                _ => throw new NotSupportedException($"Strategy {level} is not supported yet")
            };
        }
    }
}
