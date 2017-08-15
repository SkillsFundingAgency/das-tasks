using System;
using SFA.DAS.Tasks.Domain.Enums;

namespace SFA.DAS.Tasks.Domain.Models
{
    public class DasTask
    {
        public Guid Id { get; set; }
        public TaskType Type { get; set; }
        public string OwnerId { get; set; }
        public ushort ItemsDueCount { get; set; }
    }
}
