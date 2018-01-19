using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;
using SFA.DAS.Messaging.Helpers;

namespace SFA.DAS.Tasks.AcceptanceTests.DependencyResolution
{
    public class AzureTopicMessageBus : IAzureTopicMessageBus
    {
        private readonly string _manageApprenticeshipsServiceBus;
        private readonly string _commitmentsServiceBus;        

        public AzureTopicMessageBus(string manageApprenticeshipsServiceBus, string commitmentsServiceBus)
        {
            _manageApprenticeshipsServiceBus = manageApprenticeshipsServiceBus;
            _commitmentsServiceBus = commitmentsServiceBus;
        }

        public async Task<BrokeredMessage> PeekAsync(object message)
        {
            var messageGroupName = MessageGroupHelper.GetMessageGroupName(message);
            var connectionString = GetConnectionString(message);

            TopicClient client = null;

            try
            {
                client = TopicClient.CreateFromConnectionString(connectionString, messageGroupName);
                return await client.PeekAsync();
            }
            catch (System.Exception ex)
            {
                throw ex;
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

            await PublishAsync(message, messageGroupName);
        }

        public async Task PublishAsync(object message, string messageGroupName)
        {
            var connectionString = GetConnectionString(message);

            TopicClient client = null;

            try
            {
                client = TopicClient.CreateFromConnectionString(connectionString, messageGroupName);
                await client.SendAsync(new BrokeredMessage(message));
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (client != null && !client.IsClosed)
                {
                    await client.CloseAsync();
                }
            }
        }

        private string GetConnectionString(object message)
        {
            return message.GetType().FullName.Contains("EmployerAccounts") ? _manageApprenticeshipsServiceBus : _commitmentsServiceBus;
        }
    }
}
