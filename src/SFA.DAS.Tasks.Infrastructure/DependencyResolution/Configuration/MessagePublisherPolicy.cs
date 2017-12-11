using System;
using System.Linq;
using System.Reflection;
using SFA.DAS.Messaging.AzureServiceBus;
using SFA.DAS.Messaging.FileSystem;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.Tasks.Domain.Configurations;
using StructureMap.Pipeline;

namespace SFA.DAS.Tasks.Infrastructure.DependencyResolution.Configuration
{
    public class MessagePublisherPolicy<T> : MessageServiceBusPolicyBase<T> where T : ITaskConfiguration
    {
        public MessagePublisherPolicy(string serviceName): base(serviceName)
        {
            
        }

        protected override void apply(Type pluginType, IConfiguredInstance instance)
        {
            var messagePublisher = GetMessagePublisherParameter(instance);

            if (messagePublisher == null) return;

            var environment = GetEnvironmentName();
            var connectionStringName = GetConnectionStringName(instance);

            var messageQueueConnectionString = GetMessageQueueConnectionString(environment, connectionStringName);

            if (string.IsNullOrEmpty(messageQueueConnectionString))
            {
                var groupFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/EAS_Queues/";

                var publisher = new FileSystemMessagePublisher(groupFolder); 
                instance.Dependencies.AddForConstructorParameter(messagePublisher, publisher);
            }
            else
            {
                var publisher = new TopicMessagePublisher(messageQueueConnectionString);
                instance.Dependencies.AddForConstructorParameter(messagePublisher, publisher);
            }   
        }

        private static ParameterInfo GetMessagePublisherParameter(IConfiguredInstance instance)
        {
            var messagePublisher = instance?.Constructor?
                .GetParameters().FirstOrDefault(x => x.ParameterType == typeof(IMessagePublisher));
            return messagePublisher;
        }
    }
}