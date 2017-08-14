using MediatR;
using SFA.DAS.Tasks.Domain.Enums;

namespace SFA.DAS.Tasks.Application.Queries.GetTask
{
    public class GetTaskRequest : IAsyncRequest<GetTaskResponse>
    {
        public string OwnerId { get; set; }
        public TaskType Type { get; set; }
    }
}
