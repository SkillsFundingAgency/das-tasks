using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.API.Types.DTOs;

namespace SFA.DAS.Tasks.API.Client
{
    public class TaskApiClient : ITaskApiClient
    {
        private readonly ITaskApiConfiguration _configuration;
        private readonly SecureHttpClient _httpClient;
        private readonly ILog _logger;

        public TaskApiClient(ITaskApiConfiguration configuration, ILog logger)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = new SecureHttpClient(configuration);
        }

        public async Task<IEnumerable<TaskDto>> GetTasks(string employerAccountId, string userId)
        {
            var baseUrl = GetBaseUrl();
            var url = $"{baseUrl}api/tasks/{employerAccountId}/{userId}";

            _logger.Info($"Get: {url}");
            var json = await _httpClient.GetAsync(url);
            return JsonConvert.DeserializeObject<IEnumerable<TaskDto>>(json);
        }

        public async Task AddUserReminderSupression(string employerAccountId, string userId, string taskType)
        {
            var baseUrl = GetBaseUrl();
            var url = $"{baseUrl}api/tasks/{employerAccountId}/supressions/{userId}/add/{taskType}";

            _logger.Info($"Post: {url}");
            await _httpClient.PostAsync(url, new StringContent(string.Empty));
        }

        private string GetBaseUrl()
        {
            return _configuration.ApiBaseUrl.EndsWith("/")
                ? _configuration.ApiBaseUrl
                : _configuration.ApiBaseUrl + "/";
        }
    }
}
