using System;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Domain.Models
{
    public class DasTask
    {
        public Guid Id { get; set; }
        public TaskType Type { get; set; }
        public string EmployerAccountId { get; set; }
        public ushort ItemsDueCount { get; set; }
    }
}
