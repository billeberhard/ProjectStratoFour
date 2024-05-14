using System;
using System.Collections.Generic;
using System.Text;

namespace StratoFour.Application.Board
{
    public class GameBoardNavigationHelper
    {
        public static GameBoardCell GetNextCellOnRight(GameBoardCell currentCell)
        {
            return new GameBoardCell(currentCell.Column + 1, currentCell.Row);
        }

        public static GameBoardCell GetNextCellBelow(GameBoardCell currentCell)
        {
            return new GameBoardCell(currentCell.Column, currentCell.Row + 1);
        }

        public static GameBoardCell GetNextCellOnRightCornerAbove(GameBoardCell currentCell)
        {
            return new GameBoardCell(currentCell.Column + 1, currentCell.Row - 1);
        }

        public static GameBoardCell GetNextCellOnRightCornerBelow(GameBoardCell currentCell)
        {
            return new GameBoardCell(currentCell.Column + 1, currentCell.Row + 1);
        }
    }
}
