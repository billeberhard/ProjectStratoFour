using StratoFour.Application.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Application.GameStrategies
{
    public class MultiPlayerMode : IGameMode
    {
        private readonly IGameBoard _board; 

        public MultiPlayerMode(IGameBoard board)
        {
            _board = board;
        }

        public GameModeLevel GetLevel() => GameModeLevel.MultiPlayer; 

        public (int, int) Play(Player currentPlayer, Player opponentPlayer)
        {

            return (0, 0); 
        }
    }
}
