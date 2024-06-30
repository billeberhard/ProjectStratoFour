using StratoFour.Application.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Application.GameStrategies
{
    public class ApiStrategy : IGameMode
    {
        private readonly IGameBoard _board; 
        public ApiStrategy(IGameBoard board)
        {
            _board = board;
        }
        public GameModeLevel GetLevel()
        {
            return GameModeLevel.ApiPlayer;
        }

        public (int, int) Play(Player currentPlayer, Player opponentPlayer)
        {
            var column = _board.GetColumnToWin(currentPlayer);
            if (column >= 0)
            {
                var row = _board.DropDisc(column, currentPlayer);
                return (column, row);
            }

            column = _board.GetColumnToWin(opponentPlayer);
            if (column >= 0)
            {
                var row = _board.DropDisc(column, currentPlayer);
                return (column, row);
            }

            column = GetRandomColumn();
            var droppedRow = _board.DropDisc(column, currentPlayer);
            return (column, droppedRow);
        }

        private int GetRandomColumn()
        {
            var availableColumns = _board.ColumnIndices()
                .Where(column => !_board.IsColumnFull(column))
                .ToList();

            var random = new Random();
            var index = random.Next(0, availableColumns.Count);

            return availableColumns[index];
        }
    }
}
