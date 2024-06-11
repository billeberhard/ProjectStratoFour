using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using StratoFour.Application.UserMatching;

namespace StratoFour.WebUI.server.Hubs
{
    public class AuthService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IUserService _userService;

        public AuthService(AuthenticationStateProvider authenticationStateProvider, IUserService userService)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _userService = userService;
        }

        public async Task<string> GetEmailAsync(/*AuthenticationState authState*/)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                return user.FindFirst(ClaimTypes.Email)?.Value;
            }
            return null;
        }

        public async Task<string> GetUsernameAsync(/*AuthenticationState authState*/)
        {
            var email = await GetEmailAsync(/*authState*/);
            if (!string.IsNullOrEmpty(email))
            {
                var user = await _userService.GetUserByEmailAsync(email);
                return user?.Username;
            }

            return null;
        }
    }
}
