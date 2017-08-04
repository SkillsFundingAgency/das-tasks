using SFA.DAS.NLog.Logger;
using SFA.DAS.Sql.Client;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.Infrastructure.Data.Repositories
{
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        public TaskRepository(string connectionString, ILog logger) : base(connectionString, logger)
        {

        }
    }
}
