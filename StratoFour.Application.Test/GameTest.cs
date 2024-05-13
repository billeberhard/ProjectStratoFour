using FluentAssertions;
using System;
using StratoFour.Application;
using Xunit;
using System.Numerics;
using StratoFour.Application.GameStrategies;

namespace ConnectFourLogic.Test
{
    public class GameTest
    {
        private readonly IGame game;
        private readonly Player playerOne;
        private readonly Player playerTwo;

        public GameTest()
        {
            playerOne = new Player("Player 1", "#fff");
            playerTwo = new Player("Player 2", "#000");
            game = new Game(playerOne, playerTwo, GameModeLevel.Easy);
        }
    }
}