using System;
using System.Collections.Generic;
using System.Text;
using StratoFour.Application.Board;
using StratoFour.Application.GameStrategies;
using StratoFour.Application;
using StratoFour.Domain;
using System.Windows;
using System.Media;
using MQTTnet.Client;
using System.Numerics;
using MQTTnet.Client.Options;


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
        private readonly BackGroundWorkerService _mqttService;
        private readonly Action<int> _onMove;
        private readonly Action<bool> _lockGameUi;
        private readonly MessageService _messageService;

        public Game(Player playerOne, Player playerTwo, GameModeLevel level,  Action<int> onMove = null, Action<bool> lockGameUi = null, MessageService messageService)
        {
            _playerOne = playerOne;
            _playerTwo = playerTwo;
            _currentPlayer = _playerOne;

            _board = new GameBoard();
            _strategy = GameModeFactory.Create(level, _board);
            //_mqttService = mqttService;
            _onMove = onMove;
            _lockGameUi = lockGameUi;
            _messageService = messageService;
        }

        private void SubscribeToMessages()
        {
            _messageService.MessageStream.Subscribe(message =>
            {
                // Handle the received message here
                Console.WriteLine($"Game received message: {message}");
            });
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

        public async Task DropDisc(int column)
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
            var playerNumber = 1;
            if (_currentPlayer == _playerTwo)
            {
                playerNumber = 2;
            }
            _lockGameUi?.Invoke(true);
            string soundFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sounds", "playerturn_wood.wav");
            SoundPlayer sound = new SoundPlayer(soundFilePath);
            sound.PlaySync();
            // =================================================== Hack zone ==============================================
            var mqttCol = column + 1;
            await SendMqttMessageAsync("1$" + mqttCol);

            //=======================================================================================================================
            //await _mqttService.SendPlayerTurnAsync(playerNumber, column + 1);
            CheckGameStatus(column, droppedRow);
            if (_isOver)
            {
                _lockGameUi?.Invoke(false);
                return;
            }

            _onMove?.Invoke(column);

            SwitchPlayer();

            if (_strategy.GetLevel() != GameModeLevel.MultiPlayer)
            {
                (int playedColumn, int playedRow) = _strategy.Play(_currentPlayer, GetOpponent());
                // =================================================== Hack zone ==============================================
                var mqttBotCol = playedColumn + 1;
                await SendMqttMessageAsync("2$" + mqttBotCol);

                //=======================================================================================================================
                CheckGameStatus(playedColumn, playedRow);

                SwitchPlayer();
                
            }
            _lockGameUi?.Invoke(false);
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

        private async Task SendMqttMessageAsync(string payload)
        {
            var factory = new MQTTnet.MqttFactory();
            var mqttClient = factory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            try
            {
                await mqttClient.ConnectAsync(options);
                var message = new MQTTnet.MqttApplicationMessageBuilder()
                    .WithTopic("send_from_app/position")
                    .WithPayload(payload)
                    .WithRetainFlag()
                    .Build();

                await mqttClient.PublishAsync(message);
                await mqttClient.DisconnectAsync();
            }
            catch (Exception ex)
            {
                // Handle exception (log it, notify the user, etc.)
                Console.WriteLine($"MQTT operation failed: {ex.Message}");
            }
        }

    }
}
