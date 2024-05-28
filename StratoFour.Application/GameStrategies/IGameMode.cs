using StratoFour.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace StratoFour.Application.GameStrategies
{
    public interface IGameMode
    {
        GameModeLevel GetLevel();

        (int, int) Play(Player currentPlayer, Player opponentPlayer);
    }
}
