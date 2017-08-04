using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Sql.Client;
using SFA.DAS.Tasks.Domain.Repositories;
using Task = SFA.DAS.Tasks.Domain.Models.Task;

namespace SFA.DAS.Tasks.Infrastructure.Repositories
{
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        public TaskRepository(string connectionString, ILog logger) : base(connectionString, logger)
        {

        }

        public async Task<IEnumerable<Task>> GetTasks(string ownerId)
        {
            return await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ownerId", ownerId, DbType.String);
                
                return await c.QueryAsync<Task>(
                    sql: "[tasks].[GetTasksByOwnerId]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
