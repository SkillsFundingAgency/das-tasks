﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Domain.Models;

namespace SFA.DAS.Tasks.Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<DasTask>> GetTasks(string ownerId);

        Task<DasTask> GetTask(string ownerId, TaskType type);

        Task<IEnumerable<DasTask>> GetMonthlyReminderTasks();

        Task SaveTask(DasTask task);
    }
}
