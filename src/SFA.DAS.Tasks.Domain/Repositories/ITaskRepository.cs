using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Tasks.Domain.Enums;
using Todo = SFA.DAS.Tasks.Domain.Models.Todo;

namespace SFA.DAS.Tasks.Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Todo>> GetTasks(string ownerId);

        Task<Todo> GetTask(string ownerId, TodoType type);

        void SaveTask(Todo task);
    }
}
