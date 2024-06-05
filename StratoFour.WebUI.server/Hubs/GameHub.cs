using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using StratoFour.Application.UserMatching;
using StratoFour.Infrastructure.Data;
using StratoFour.Infrastructure.Models;

namespace StratoFour.WebUI.server.Hubs
{
    public class GameHub : Hub
    {
        private readonly IUserService _userService;
        private readonly IGameService _gameService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GameHub> _logger;
        private readonly AuthService _authService;

        public GameHub(IUserService userService, IGameService gameService, IHttpContextAccessor httpContextAccessor, ILogger<GameHub> logger, AuthService authService)
        {
            _userService = userService;
            _gameService = gameService;
            _httpContextAccessor = httpContextAccessor; 
            _logger = logger;
            _authService = authService;
        }

        public async Task ConnectUser(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("User is not authenticated.");
                    throw new Exception("User is not authenticated.");
                }

                var user = await _userService.GetUserByEmailAsync(email);
                _logger.LogInformation($"Connecting user: {user.Username}");

                user.ConnectionId = Context.ConnectionId;
                await _userService.UpdateUserConnectionIdAsync(user.Id, Context.ConnectionId);
                await Clients.Caller.SendAsync("ReceiveUserId", user.Id);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error in ConnectUser");
            }
        }

        public async Task StartGame(int player1Id, int player2Id, int robotId)
        {
            try
            {
                var sessionId = await _gameService.CreateGameAsync(player1Id, player2Id, robotId);
                var player1 = await _userService.GetUserByIdAsync(player1Id);
                var player2 = await _userService.GetUserByIdAsync(player2Id);

                await Clients.Client(player1.ConnectionId).SendAsync("StartGame", sessionId, "Player1");
                await Clients.Client(player2.ConnectionId).SendAsync("StartGame", sessionId, "Player2");
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "Error in StartGame");
            }
        }

        public async Task JoinGame(int userId)
        {
            try
            {
                var session = await _gameService.GetActiveGameSessionAsync(userId);
                if (session != null)
                {
                    await Clients.Caller.SendAsync("JoinGame", session.SessionId);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in JoinGame");
            }   

        }

        public async Task MakeMove(int column)
        {
            await Clients.All.SendAsync("MoveMade", column);
        }

        public async Task SendMatchRequest(string user)
        {
            await Clients.User(user).SendAsync("MatchRequestReceived", Context.User.Identity.Name);
        }

        public async Task ConfirmMatch(string user)
        {
            await Clients.User(user).SendAsync("MatchConfirmed", Context.User.Identity.Name);
        }

    }
}
