using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using StratoFour.Application.UserMatching;
using StratoFour.Infrastructure.Data;
using StratoFour.Infrastructure.Models;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Transactions;

namespace StratoFour.WebUI.server.Hubs
{
    public class GameHub : Hub
    {
        private static ConcurrentQueue<int> _playerQueue = new ConcurrentQueue<int>();

        private readonly IUserService _userService;
        private readonly IGameService _gameService;
        private readonly IRobotService _robotService;
        private readonly ILogger<GameHub> _logger;
        private readonly AuthService _authService;

        public GameHub(IUserService userService, IGameService gameService, IRobotService robotService, ILogger<GameHub> logger, AuthService authService)
        {
            _userService = userService;
            _gameService = gameService;
            _robotService = robotService;
            _logger = logger;
            _authService = authService;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("Client connected: " + Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation("Client disconnected: " + Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task ConnectUser(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("User is not authenticated.");
                    throw new HubException("User is not authenticated.");
                }


                var user = await _userService.GetUserByEmailAsync(email);
                _logger.LogInformation($"Connecting user: {user.Username}");


                user.ConnectionId = Context.ConnectionId;
                await _userService.UpdateUserConnectionIdAsync(user.UserId, Context.ConnectionId);
                await Clients.Caller.SendAsync("ReceiveUserId", user.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ConnectUser");
                throw new HubException("An error occurred while connecting the user.", ex);
            }
        }

        public async Task FindGame(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            _playerQueue.Enqueue(user.UserId);

            await Clients.Caller.SendAsync("WaitingForOpponent");

            if (_playerQueue.Count >= 2)
            {
                if (_playerQueue.TryDequeue(out var player1Id) && _playerQueue.TryDequeue(out var player2Id))
                {
                    var player1 = await _userService.GetUserByIdAsync(player1Id);
                    var player2 = await _userService.GetUserByIdAsync(player2Id);

                    await Clients.Client(player1.ConnectionId).SendAsync("MatchFound", player2);
                    await Clients.Client(player2.ConnectionId).SendAsync("MatchFound", player1);
                }
            }
        }

        public async Task AcceptMatch(int userId, int otherUserId)
        {
            RobotModel? readyRobot = null;
            using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    readyRobot = await _robotService.GetReadyRobot();
                    if (readyRobot == null)
                    {
                        throw new HubException("No ready robot available.");
                    }

                    var sessionId = await _gameService.CreateGameAsync(userId, otherUserId, readyRobot.RobotId);
                    await _robotService.UpdateRobotStatus(readyRobot.RobotId, "In Use");

                    var player1 = await _userService.GetUserByIdAsync(userId);
                    var player2 = await _userService.GetUserByIdAsync(otherUserId);

                    await Clients.Client(player1.ConnectionId).SendAsync("StartGame", sessionId, "Player1");
                    await Clients.Client(player2.ConnectionId).SendAsync("StartGame", sessionId, "Player2");

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in AcceptMatch");

                    // Rollback: Set the robot status back to "Ready"
                    if (readyRobot != null)
                    {
                        await _robotService.UpdateRobotStatus(readyRobot.RobotId, "Ready");
                    }

                    throw new HubException("An error occurred while accepting the match.", ex);

                }
            }
        }

        public async Task DeclineMatch(int userId)
        {
            await Clients.Caller.SendAsync("ChooseGameMode");
        }

    }
}
