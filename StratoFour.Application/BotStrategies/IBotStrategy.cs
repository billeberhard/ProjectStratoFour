using StratoFour.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace StratoFour.Application.BotStrategies
{
    public interface IBotStrategy
    {
        BotStrategyLevel GetLevel();

        (int, int) Play(Player currentPlayer, Player opponentPlayer);
    }
}
