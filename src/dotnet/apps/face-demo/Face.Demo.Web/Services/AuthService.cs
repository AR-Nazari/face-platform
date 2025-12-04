using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Face.Application.Auth.Login;

namespace Face.Demo.Web.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public string? JwtToken { get; private set; }

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResultDto?> LoginAsync(string userName, string password)
        {
            var payload = new
            {
                userName,
                password
            };

            var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/login", payload);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResultDto>();
            if (result != null)
            {
                JwtToken = result.Token;
            }

            return result;
        }

        public void Logout()
        {
            JwtToken = null;
        }
    }
}
