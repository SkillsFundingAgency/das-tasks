using System;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Domain.Models
{
    public class UserReminderSupressionFlag
    {
        public long UserId { get; set; }
        public long AccountId { get; set; }
        public TaskType ReminderType { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
