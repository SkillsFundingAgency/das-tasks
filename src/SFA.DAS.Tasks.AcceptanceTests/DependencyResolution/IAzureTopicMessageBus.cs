using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;

namespace SFA.DAS.Tasks.AcceptanceTests.DependencyResolution
{
    public interface IAzureTopicMessageBus
    {
        Task PublishAsync(object message);

        Task<BrokeredMessage> PeekAsync(object message);
    }
}
