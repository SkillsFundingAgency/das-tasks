using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Domain.Models;

namespace SFA.DAS.Tasks.Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<DasTask>> GetTasks(string employerAccountId);

        Task<DasTask> GetTask(string employerAccountId, TaskType type);

        Task<IEnumerable<DasTask>> GetMonthlyReminderTasks(string employerAccountId);

        Task SaveUserReminderSuppression(UserReminderSuppressionFlag flag);

        Task SaveTask(DasTask task);

        Task<IEnumerable<TaskType>> GetUserTaskSuppressions(string userId, string employerAccountId);
    }
}
