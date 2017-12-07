
using SFA.DAS.Messaging.AzureServiceBus.StructureMap;

namespace SFA.DAS.Tasks.AcceptenceTests.Configuration
{
    public class TasksMessagePublishConfiguration : ITopicMessagePublisherConfiguration
    {
        public string MessageServiceBusConnectionString { get; set; }
    }
}