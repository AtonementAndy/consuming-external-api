using System.Net.Http.Json;
using CallWebApi.Configuration;
using CallWebApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CallWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserService> _logger;
        private readonly string _baseUrl = string.Empty;

        public UserService(HttpClient httpClient, ILogger<UserService> logger, IOptions<ApiSettings> settings)
        {
            _httpClient = httpClient;
            _logger = logger;
            
            _baseUrl = settings.Value.BaseUrl;
            if (string.IsNullOrEmpty(_baseUrl)){
                _baseUrl = "https://jsonplaceholder.typicode.com/";
            };
        }

        public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting to seek the user requisition at {Url}", _baseUrl);

            try
            {
                var fullUrl = $"{_baseUrl.TrimEnd('/')}/users";
                
                var users = await _httpClient.GetFromJsonAsync<IEnumerable<User>>(fullUrl, cancellationToken);

                if (users == null)
                {
                    _logger.LogWarning("The API returned a null list.");
                    return [];
                }

                _logger.LogInformation("{Count} users retrieved successfully.", users.Count());
                return users;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex,"Communication with the external API failed.");
                throw;
            }
        }
    }
}