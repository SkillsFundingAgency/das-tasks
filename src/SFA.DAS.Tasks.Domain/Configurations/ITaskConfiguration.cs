using System.Collections.Generic;

namespace SFA.DAS.Tasks.Domain.Configurations
{
    public interface ITaskConfiguration
    {
        string DatabaseConnectionString { get; set; }
        string ServiceBusConnectionString { get; set; }
        string MessageServiceBusConnectionString { get; set; }
        Dictionary<string, string> MessageServiceBusConnectionStringLookup { get; set; }
    }
}
