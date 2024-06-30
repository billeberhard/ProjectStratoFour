using StratoFour.RestApi.Controllers;

namespace StratoFour.RestApi
{
    public class ApiGameTester
    {
        private readonly HttpClient _httpClient;

        public ApiGameTester(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task MakeRandomMoveAsync()
        {
            var gameId = 99999; 
            var random = new Random();
            var column = random.Next(0, 7);

            var response = await _httpClient.PostAsJsonAsync($"api/gamemove/move?gameId={gameId}&column={column}", new { });
            response.EnsureSuccessStatusCode();
        }

        //public async Task ConfirmGameAsync(int gameId)
        //{
        //    var confirmGameRequest = new ConfirmGameRequest
        //    {
        //        GameId = gameId
        //    };

        //    var response = await _httpClient.PostAsJsonAsync("api/game/confirm", confirmGameRequest);
        //    response.EnsureSuccessStatusCode();
        //}
    }
}

