using System.Collections.Generic;
using SFA.DAS.Configuration;

namespace SFA.DAS.Tasks.Domain.Configurations
{
    public class TasksConfiguration : ITaskConfiguration
    {
        public string DatabaseConnectionString { get; set; }
        public string ServiceBusConnectionString { get; set; }
        public string MessageServiceBusConnectionString { get; set; }
        public Dictionary<string, string> MessageServiceBusConnectionStringLookup { get; set; }
    }
}
