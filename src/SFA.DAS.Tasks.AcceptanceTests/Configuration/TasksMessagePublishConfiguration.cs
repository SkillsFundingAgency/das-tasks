
using SFA.DAS.Messaging.AzureServiceBus.StructureMap;
using System.Collections.Generic;

namespace SFA.DAS.Tasks.AcceptanceTests.Configuration
{
    public class TasksMessagePublishConfiguration : ITopicMessagePublisherConfiguration
    {
        public string MessageServiceBusConnectionString { get; set; }

        public Dictionary<string, string> MessageServiceBusConnectionStringLookup { get; set; }
    }
}
