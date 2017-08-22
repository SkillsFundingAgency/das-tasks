using System.Collections.Generic;
using SFA.DAS.Tasks.Domain.Models;

namespace SFA.DAS.Tasks.Application.Queries.GetTasksByOwnerId
{
    public class GetTasksByOwnerIdResponse
    {
        public IEnumerable<DasTask> Tasks { get; set; }
    }
}
