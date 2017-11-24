using MediatR;

namespace SFA.DAS.Tasks.Application.Commands.SaveMonthlyUserDismiss
{
    public class SaveMonthlyReminderDismissCommand : IAsyncRequest<SaveMonthlyReminderDismissCommandResponse>
    {
        public long UserId { get; set; }
        public long AccountId { get; set; }
        public string TaskType { get; set; }
    }
}
