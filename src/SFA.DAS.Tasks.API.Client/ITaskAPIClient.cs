using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Tasks.API.Types.DTOs;

namespace SFA.DAS.Tasks.API.Client
{
    public interface ITaskApiClient
    {
        Task<IEnumerable<TaskDto>> GetTasks(string employerAccountId, string userid);

        Task AddUserReminderSupression(string employerAccountId, string userId, string taskType);
    }
}
