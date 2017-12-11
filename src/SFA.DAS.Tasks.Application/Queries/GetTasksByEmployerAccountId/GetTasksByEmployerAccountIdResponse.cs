using System.Collections.Generic;
using SFA.DAS.Tasks.Domain.Models;

namespace SFA.DAS.Tasks.Application.Queries.GetTasksByEmployerAccountId
{
    public class GetTasksByEmployerAccountIdResponse
    {
        public IEnumerable<DasTask> Tasks { get; set; }
    }
}
