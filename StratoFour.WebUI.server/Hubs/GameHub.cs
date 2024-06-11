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
        private static ConcurrentDictionary<int, MatchInfo> _pendingMatches = new ConcurrentDictionary<int, MatchInfo>();

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

                    var matchInfo = new MatchInfo
                    {
                        Player1 = player1,
                        Player2 = player2,
                        Robot = await _robotService.GetReadyRobot()
                    };

                    _pendingMatches.TryAdd(player1Id, matchInfo);
                    _pendingMatches.TryAdd(player2Id, matchInfo);

                    await Clients.Client(player1.ConnectionId).SendAsync("MatchFound", player2);
                    await Clients.Client(player2.ConnectionId).SendAsync("MatchFound", player1);
                }
            }
        }

        public async Task AcceptMatch(int userId, int otherUserId)
        {
            if (_pendingMatches.TryGetValue(userId, out var matchInfo))
            {
                if (matchInfo.AcceptedByPlayer1 && matchInfo.AcceptedByPlayer2)
                {
                    throw new HubException("Match already accepted by both players.");
                }

                if (matchInfo.Player1.UserId == userId)
                {
                    matchInfo.AcceptedByPlayer1 = true;
                }
                else if (matchInfo.Player2.UserId == userId)
                {
                    matchInfo.AcceptedByPlayer2 = true;
                }

                if (matchInfo.AcceptedByPlayer1 && matchInfo.AcceptedByPlayer2)
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        try
                        {
                            await _robotService.UpdateRobotStatus(matchInfo.Robot.RobotId, "In Use");

                            var gameId = await _gameService.CreateGameAsync(matchInfo.Player1.UserId, matchInfo.Player2.UserId, matchInfo.Robot.RobotId);

                            await Clients.Client(matchInfo.Player1.ConnectionId).SendAsync("StartGame", gameId, "Player1");
                            await Clients.Client(matchInfo.Player2.ConnectionId).SendAsync("StartGame", gameId, "Player2");

                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error in AcceptMatch");
                            await _robotService.UpdateRobotStatus(matchInfo.Robot.RobotId, "Ready");
                            throw new HubException("An error occurred while accepting the match.", ex);
                        }
                        finally
                        {
                            _pendingMatches.TryRemove(matchInfo.Player1.UserId, out _);
                            _pendingMatches.TryRemove(matchInfo.Player2.UserId, out _);
                        }
                    }
                }
            }
            else
            {
                throw new HubException("No pending match found for this user.");
            }
        }

        public async Task DeclineMatch(int userId)
        {
            if(_pendingMatches.TryGetValue(userId, out var matchInfo))
            {
                await Clients.Client(matchInfo.Player1.ConnectionId).SendAsync("MatchDeclined");
                await Clients.Client(matchInfo.Player2.ConnectionId).SendAsync("MatchDeclined");

                await _robotService.UpdateRobotStatus(matchInfo.Robot.RobotId, "Ready");

                _pendingMatches.TryRemove(matchInfo.Player1.UserId, out _);
                _pendingMatches.TryRemove(matchInfo.Player2.UserId, out _);
            }
            await Clients.Caller.SendAsync("ChooseGameMode");
        }

    }
}
