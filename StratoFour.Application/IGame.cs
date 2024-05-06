using StratoFour.Application.Board;
using StratoFour.Application;

namespace StratoFour.Application
{
    public interface IGame
    {
        IGameBoard GetBoard();

        Player GetCurrentPlayer();

        Player GetWinner();

        bool IsOver();

        void DropDisc(int column);
    }
}