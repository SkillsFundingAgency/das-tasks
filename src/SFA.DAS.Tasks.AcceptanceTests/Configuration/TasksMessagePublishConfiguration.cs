
using SFA.DAS.Messaging.AzureServiceBus.StructureMap;

namespace SFA.DAS.Tasks.AcceptanceTests.Configuration
{
    public class TasksMessagePublishConfiguration : ITopicMessagePublisherConfiguration
    {
        public string MessageServiceBusConnectionString { get; set; }
    }
}
