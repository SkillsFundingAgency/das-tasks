using System;
using System.Data;
using System.Threading.Tasks;

namespace SFA.DAS.Tasks.AcceptanceTests.Repository
{
    public interface ITaskRepository
    {
        Task WithConnection(Func<IDbConnection, Task> query);
    }
}
