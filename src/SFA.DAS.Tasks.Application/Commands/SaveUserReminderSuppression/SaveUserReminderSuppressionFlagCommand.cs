using MediatR;

namespace SFA.DAS.Tasks.Application.Commands.SaveUserReminderSuppression
{
    public class SaveUserReminderSuppressionFlagCommand : IAsyncRequest<SaveUserReminderSuppressionFlagCommandResponse>
    {
        public string UserId { get; set; }
        public string EmployerAccountId { get; set; }
        public string TaskType { get; set; }
    }
}
