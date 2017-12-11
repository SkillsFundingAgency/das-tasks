using MediatR;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Application.Commands.SaveTask
{
    public class SaveTaskCommand : IAsyncRequest<SaveTaskCommandResponse>
    {
        public TaskType Type { get; set; }
        public string EmployerAccountId { get; set; }
        public bool TaskCompleted { get; set; }
        public bool CompleteAllTasks { get; set; }
    }
}
