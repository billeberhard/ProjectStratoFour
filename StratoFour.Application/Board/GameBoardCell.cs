namespace StratoFour.Application.Board
{
    public class GameBoardCell
    {
        public int Column { get; }

        public int Row { get; }

        public GameBoardCell(int column, int row)
        {
            Column = column;
            Row = row;
        }


        public override bool Equals(object obj)
        {
            var item = obj as GameBoardCell;

            if (item == null)
            {
                return false;
            }

            return Column == item.Column && Row == item.Row;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Column * 397) ^ Row;
            }
        }
    }
}
