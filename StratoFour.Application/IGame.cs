using StratoFour.Application.Board;
using StratoFour.Application;
using StratoFour.Application.GameStrategies;

namespace StratoFour.Application
{
    public interface IGame
    {
        GameModeLevel GetGameModeLevel();
        IGameBoard GetBoard();
        Player GetCurrentPlayer();
        Player GetWinner();
        bool IsOver();
        void DropDisc(int column);
    }
}