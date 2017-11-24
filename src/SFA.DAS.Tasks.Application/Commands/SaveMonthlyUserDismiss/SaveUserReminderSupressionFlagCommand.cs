using MediatR;

namespace SFA.DAS.Tasks.Application.Commands.SaveMonthlyUserDismiss
{
    public class SaveUserReminderSupressionFlagCommand : IAsyncRequest<SaveUserReminderSupressionFlagCommandResponse>
    {
        public string UserId { get; set; }
        public string AccountId { get; set; }
        public string TaskType { get; set; }
    }
}
