using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Tasks.API.Types.DTOs;

namespace SFA.DAS.Tasks.API.Client
{
    public class TaskApiClient : ITaskApiClient
    {
        private readonly ITaskApiConfiguration _configuration;
        private readonly SecureHttpClient _httpClient;

        public TaskApiClient(ITaskApiConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new SecureHttpClient(configuration);
        }

        public async Task<IEnumerable<TaskDto>> GetTasks(string ownerId, string userId)
        {
            var baseUrl = GetBaseUrl();
            var url = $"{baseUrl}api/tasks/{ownerId}/{userId}";

            var json = await _httpClient.GetAsync(url);
            return JsonConvert.DeserializeObject<IEnumerable<TaskDto>>(json);
        }

        private string GetBaseUrl()
        {
            return _configuration.ApiBaseUrl.EndsWith("/")
                ? _configuration.ApiBaseUrl
                : _configuration.ApiBaseUrl + "/";
        }
    }
}
