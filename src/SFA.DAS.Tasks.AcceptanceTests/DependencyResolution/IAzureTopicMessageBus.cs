using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;

namespace SFA.DAS.Tasks.AcceptanceTests.DependencyResolution
{
    public interface IAzureTopicMessageBus
    {
        Task PublishAsync(object message);

        Task PublishAsync(object message, string messageGroupName);

        Task<BrokeredMessage> PeekAsync(object message);
    }
}
