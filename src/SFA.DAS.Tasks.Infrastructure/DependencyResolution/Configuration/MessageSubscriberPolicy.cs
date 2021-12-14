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
                var logger = new NLogLogger(typeof(TopicSubscriberFactory));

                var subscriptionName = TopicSubscriptionHelper.GetMessageGroupName(instance.Constructor.DeclaringType);

                logger.Info($"Applying MessageSubscriberPolicy for subscription {subscriptionName}");

                var useManagedIdentity = !messageQueueConnectionString.Contains("SharedAccessKey");
                logger.Info($"Using ManagedIdentity: {useManagedIdentity}");
                if (useManagedIdentity && !messageQueueConnectionString.StartsWith("sb://"))
                {
                    messageQueueConnectionString = $"sb://{messageQueueConnectionString}";
                }
                
                var factory = new TopicSubscriberFactory(messageQueueConnectionString, subscriptionName, logger, false, useManagedIdentity);

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