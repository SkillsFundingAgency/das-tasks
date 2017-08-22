using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Tasks.Domain.Enums;
using SFA.DAS.Tasks.Domain.Models;

namespace SFA.DAS.Tasks.Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<DasTask>> GetTasks(string ownerId);

        Task<DasTask> GetTask(string ownerId, TaskType type);

        Task SaveTask(DasTask task);
    }
}
