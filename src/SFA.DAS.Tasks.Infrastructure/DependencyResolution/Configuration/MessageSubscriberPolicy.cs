using System;
using System.Linq;
using System.Reflection;
using SFA.DAS.Messaging.AzureServiceBus;
using SFA.DAS.Messaging.AzureServiceBus.Helpers;
using SFA.DAS.Messaging.FileSystem;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Domain.Configurations;
using StructureMap.Pipeline;

namespace SFA.DAS.Tasks.Infrastructure.DependencyResolution.Configuration 
{
    public class MessageSubscriberPolicy<T> : MessageServiceBusPolicyBase<T> where T : ITaskConfiguration
    {
        public MessageSubscriberPolicy(string serviceName) : base(serviceName)
        {
           
        }

        protected override void apply(Type pluginType, IConfiguredInstance instance)
        {
            var subscriberFactory = GetMessageSubscriberFactoryParameter(instance);

            if (subscriberFactory == null) return;

            var environment = GetEnvironmentName();
            var connectionStringName = GetConnectionStringName(instance);

            var messageQueueConnectionString = GetMessageQueueConnectionString(environment, connectionStringName);

            if (string.IsNullOrEmpty(messageQueueConnectionString))
            {
                var groupFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/EAS_Queues/";
                var factory = new FileSystemMessageSubscriberFactory(groupFolder);

                instance.Dependencies.AddForConstructorParameter(subscriberFactory, factory);
            }
            else
            {
                var subscriptionName = TopicSubscriptionHelper.GetMessageGroupName(instance.Constructor.DeclaringType);

                var useManagedIdentity = !messageQueueConnectionString.Contains("SharedAccessKey");

                var factory = new TopicSubscriberFactory(messageQueueConnectionString, subscriptionName, new NLogLogger(typeof(TopicSubscriberFactory)), false, useManagedIdentity);

                instance.Dependencies.AddForConstructorParameter(subscriberFactory, factory);
            }   
        }

        private static ParameterInfo GetMessageSubscriberFactoryParameter(IConfiguredInstance instance)
        {
            var factory = instance?.Constructor?
                .GetParameters().FirstOrDefault(x => x.ParameterType == typeof(IMessageSubscriberFactory));

            return factory;
        }
    }
}