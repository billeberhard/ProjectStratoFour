using StratoFour.Application.Board;
using StratoFour.Application;
using StratoFour.Application.GameStrategies;

namespace StratoFour.Application
{
    public interface IGame
    {
        //void SubscribeToMessages(); 
        GameModeLevel GetGameModeLevel();
        IGameBoard GetBoard();
        Player GetCurrentPlayer();
        Player GetWinner();
        bool IsOver();
        Task DropDisc(int column);
    }
}