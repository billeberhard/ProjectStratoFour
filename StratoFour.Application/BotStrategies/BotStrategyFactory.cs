using StratoFour.Application.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Application.BotStrategies
{
    class BotStrategyFactory
    {
        public static IBotStrategy Create(BotStrategyLevel level, IGameBoard board)
        {
            return level switch
            {
                BotStrategyLevel.Easy => new EasyLevelStrategy(board),
                _ => throw new NotSupportedException($"Strategy {level} is not supported yet")
            };
        }
    }
}
