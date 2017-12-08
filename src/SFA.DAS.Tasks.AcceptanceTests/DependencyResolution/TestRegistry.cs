using SFA.DAS.Tasks.AcceptanceTests.Configuration;
using SFA.DAS.Tasks.AcceptanceTests.Repository;
using SFA.DAS.Tasks.API.Client;
using SFA.DAS.Tasks.Infrastructure.DependencyResolution.Configuration;
using StructureMap;

namespace SFA.DAS.Tasks.AcceptanceTests.DependencyResolution
{
    public class TestRegistry : Registry
    {
        public TestRegistry()
        {
            var taskapiconfig = ConfigurationHelper.GetConfiguration<TaskApiConfiguration>($"SFA.DAS.Tasks.Api");
            For<ITaskApiClient>().Use(new TaskApiClient(taskapiconfig));
            var messagePublisher = ConfigurationHelper.GetConfiguration<TasksMessagePublishConfiguration>($"SFA.DAS.Tasks");
            For<IAzureTopicMessageBus>().Use(new AzureTopicMessageBus(messagePublisher.MessageServiceBusConnectionString));
            var tasksDbConfiguration = ConfigurationHelper.GetConfiguration<TasksDbConfiguration>($"SFA.DAS.Tasks");
            For<ITaskRepository>().Use<TaskRepository>().Ctor<string>().Is(tasksDbConfiguration.DatabaseConnectionString);
        }
    }
}
