using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;
using SFA.DAS.Messaging.Helpers;

namespace SFA.DAS.Tasks.AcceptanceTests.DependencyResolution
{
    public class AzureTopicMessageBus : IAzureTopicMessageBus
    {
        private readonly string _connectionString;

        public AzureTopicMessageBus(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<BrokeredMessage> PeekAsync(object message)
        {
            var messageGroupName = MessageGroupHelper.GetMessageGroupName(message);

            TopicClient client = null;

            try
            {
                client = TopicClient.CreateFromConnectionString(_connectionString, messageGroupName);
                return await client.PeekAsync();
            }
            finally
            {
                if (client != null && !client.IsClosed)
                {
                    await client.CloseAsync();
                }
            }
        }

        public async Task PublishAsync(object message)
        {
            var messageGroupName = MessageGroupHelper.GetMessageGroupName(message);

            TopicClient client = null;

            try
            {
                client = TopicClient.CreateFromConnectionString(_connectionString, messageGroupName);
                await client.SendAsync(new BrokeredMessage(message));
            }
            finally
            {
                if (client != null && !client.IsClosed)
                {
                    await client.CloseAsync();
                }
            }
        }
    }
}
