using System.Collections.Generic;
using System.Threading.Tasks;
using Task = SFA.DAS.Tasks.Domain.Models.Task;

namespace SFA.DAS.Tasks.Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Task>> GetTasks(string ownerId);
    }
}
