using MediatR;
using SFA.DAS.Tasks.Domain.Enums;

namespace SFA.DAS.Tasks.Application.Commands.SaveTask
{
    public class SaveTaskCommand : IAsyncRequest<SaveTaskCommandResponse>
    {
        public TaskType Type { get; set; }
        public string OwnerId { get; set; }
        public bool TaskCompleted { get; set; }
    }
}
