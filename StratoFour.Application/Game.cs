using System;
using System.Collections.Generic;
using System.Text;
using StratoFour.Application.Board;
using StratoFour.Application.GameStrategies;
using StratoFour.Application;

namespace StratoFour.Application
{
    public class Game : IGame
    {
        private bool _isOver;

        private Player _currentPlayer;

        private readonly Player _playerOne;
        private readonly Player _playerTwo;
        private Player _winner;
        private readonly IGameMode _strategy;
        private readonly IGameBoard _board;
        private readonly Action<int> _onMove;

        public Game(Player playerOne, Player playerTwo, GameModeLevel level, Action<int> onMove = null)
        {
            _playerOne = playerOne;
            _playerTwo = playerTwo;
            _currentPlayer = _playerOne;

            _board = new GameBoard();
            _strategy = GameModeFactory.Create(level, _board);
            _onMove = onMove;
        }

        public GameModeLevel GetGameModeLevel()
        {
            return _strategy.GetLevel();
        }

        public IGameBoard GetBoard()
        {
            return _board;
        }

        public bool IsOver()
        {
            return _isOver;
        }
        public Player GetCurrentPlayer()
        {
            return _currentPlayer;
        }

        public Player GetWinner()
        {
            return _winner;
        }

        public void DropDisc(int column)
        {
            if (_isOver)
            {
                return;
            }

            int droppedRow = _board.DropDisc(column, _currentPlayer);
            if (droppedRow == GameBoard.InvalidRowColumn)
            {
                return;
            }

            CheckGameStatus(column, droppedRow);
            if (_isOver)
            {
                return;
            }
            
            _onMove?.Invoke(column);

            SwitchPlayer();

            if (_strategy.GetLevel() != GameModeLevel.MultiPlayer)
            {
                (int playedColumn, int playedRow) = _strategy.Play(_currentPlayer, GetOpponent());
                CheckGameStatus(playedColumn, playedRow);

                SwitchPlayer();
            }
        }

        private void SwitchPlayer()
        {
            _currentPlayer = _currentPlayer == _playerTwo ? _playerOne : _playerTwo;
        }

        private Player GetOpponent()
        {
            return _currentPlayer == _playerTwo ? _playerOne : _playerTwo;
        }

        private void CheckGameStatus(int column, int row)
        {
            var hasWon = _board.HasPlayerWon(_currentPlayer, new GameBoardCell(column, row));

            if (hasWon)
            {
                _isOver = true;
                _winner = _currentPlayer;
                return;
            }

            if (_board.IsFull())
            {
                _isOver = true;
            }
        }
    }
}
