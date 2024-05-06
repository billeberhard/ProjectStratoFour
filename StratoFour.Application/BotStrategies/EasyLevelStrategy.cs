using StratoFour.Application.GameBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratoFour.Application.BotStrategies
{
    class EasyLevelStrategy : IBotStrategy
    {
        private readonly IGameBoard _board;

        public EasyLevelStrategy(IGameBoard board)
        {
            _board = board;
        }

        public BotStrategyLevel GetLevel()
        {
            return BotStrategyLevel.Easy;
        }

        /// <summary>
        /// If there is winning chance, drop the disc in that column.
        /// Else if the opponent has the chance, try to prevent it.
        /// Otherwise, play in a random column.
        /// </summary>
        /// <param name="currentPlayer"></param>
        /// <param name="opponentPlayer"></param>
        /// <returns>Played column and row</returns>
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
