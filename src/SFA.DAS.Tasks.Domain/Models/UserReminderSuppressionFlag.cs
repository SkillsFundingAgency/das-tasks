using System;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Domain.Models
{
    public class UserReminderSuppressionFlag
    {
        public string UserId { get; set; }
        public string EmployerAccountId { get; set; }
        public TaskType ReminderType { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
