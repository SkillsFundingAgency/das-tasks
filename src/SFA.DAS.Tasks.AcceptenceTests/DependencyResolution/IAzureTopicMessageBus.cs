using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;

namespace SFA.DAS.Tasks.AcceptenceTests.DependencyResolution
{
    public interface IAzureTopicMessageBus
    {
        Task PublishAsync(object message);

        Task<BrokeredMessage> PeekAsync(object message);
    }
}