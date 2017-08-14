using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Tasks.Worker.Processors
{
    public interface ITaskMessageProcessor
    {
        Task RunAsync(CancellationToken cancellationToken);
    }
}
