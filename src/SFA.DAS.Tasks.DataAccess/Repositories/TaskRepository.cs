﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Sql.Client;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Domain.Configurations;
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

        public async Task<IEnumerable<DasTask>> GetMonthlyReminderTasks(string ownerId)
        {
            return await WithConnection(async c =>
            {
                var tasks = (await c.QueryAsync<DasTask>(
                    sql: "[tasks].[GetMonthlyReminderTasks]",
                    commandType: CommandType.StoredProcedure)).ToList();

                foreach (var task in tasks)
                {
                    task.OwnerId = ownerId;
                    task.ItemsDueCount = 1;
                }

                return tasks.ToList();
            });
        }

        public async Task SaveUserReminderSuppression(UserReminderSuppressionFlag flag)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@userId", flag.UserId, DbType.String);
                parameters.Add("@accountId", flag.AccountId, DbType.String);
                parameters.Add("@reminderTaskType", flag.ReminderType, DbType.String);

                return await c.ExecuteAsync(
                    sql: "[tasks].[AddUserReminderSuppression]",
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

        public async Task<IEnumerable<TaskType>> GetUserTaskSuppressions(string userId, string accountId)
        {
            var result = await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@accountId", accountId, DbType.String);
                parameters.Add("@userId", userId, DbType.String);

                return await c.QueryAsync<string>(
                    sql: "[Tasks].[GetUserTaskSuppressions]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });

            var taskTypes = GetTaskTypesFromStringResult(result);

            return taskTypes;
        }

        private static IEnumerable<TaskType> GetTaskTypesFromStringResult(IEnumerable<string> result)
        {
            var taskTypes = new List<TaskType>();

            foreach (var typeString in result)
            {
                //Ignore and entries that do not have a valid task type
                if (Enum.TryParse(typeString, out TaskType type))
                {
                    taskTypes.Add(type);
                }
            }
            return taskTypes;
        }
    }
}
