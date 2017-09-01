using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Sql.Client;
using SFA.DAS.Tasks.Domain.Configurations;
using SFA.DAS.Tasks.Domain.Enums;
using SFA.DAS.Tasks.Domain.Models;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.DataAccess.Repositories
{
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        public TaskRepository(TasksConfiguration configuration, ILog logger) : base(configuration.DatabaseConnectionString, logger)
        {  }

        public async Task<IEnumerable<DasTask>> GetTasks(string ownerId)
        {
            return await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ownerId", ownerId, DbType.String);
                
                return await c.QueryAsync<DasTask>(
                    sql: "[tasks].[GetTasksByOwnerId]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }

        public async Task<DasTask> GetTask(string ownerId, TaskType type)
        {
            return await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ownerId", ownerId, DbType.String);
                parameters.Add("@type", type, DbType.String);

                return await c.QuerySingleOrDefaultAsync<DasTask>(
                    sql: "[tasks].[GetTaskByOwnerIdAndType]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }

        public async Task SaveTask(DasTask task)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", task.Id, DbType.Guid);
                parameters.Add("@ownerId", task.OwnerId, DbType.String);
                parameters.Add("@type", task.Type, DbType.String);
                parameters.Add("@itemsDueCount", (int)task.ItemsDueCount, DbType.Int32);

                return await c.ExecuteAsync(
                    sql: "[tasks].[UpsertTask]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
