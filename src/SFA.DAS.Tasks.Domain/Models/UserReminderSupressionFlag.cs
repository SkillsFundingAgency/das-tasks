using System;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Domain.Models
{
    public class UserReminderSupressionFlag
    {
        public string UserId { get; set; }
        public string AccountId { get; set; }
        public TaskType ReminderType { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
