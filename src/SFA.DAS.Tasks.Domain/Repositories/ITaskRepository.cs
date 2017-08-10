using System.Collections.Generic;
using System.Threading.Tasks;
using Todo = SFA.DAS.Tasks.Domain.Models.Todo;

namespace SFA.DAS.Tasks.Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Todo>> GetTasks(string ownerId);
    }
}
