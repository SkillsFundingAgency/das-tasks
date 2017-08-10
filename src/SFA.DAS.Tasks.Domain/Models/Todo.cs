using SFA.DAS.Tasks.Domain.Enums;

namespace SFA.DAS.Tasks.Domain.Models
{
    public class Todo
    {
        public TodoType Type { get; set; }
        public string TaskOwnerId { get; set; }
        public int ItemsDueCount { get; set; }
    }
}
