using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Messaging;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Worker.MessageHandlers;

namespace SFA.DAS.Tasks.Worker.Processors
{
    public class TaskMessageProcessor<T> : ITaskMessageProcessor where T : new()
    {
        private readonly IPollingMessageReceiver _pollingMessageReceiver;
        private readonly IMessageHandler<T> _handler;
        private readonly ILog _logger;

        public TaskMessageProcessor(IPollingMessageReceiver pollingMessageReceiver, IMessageHandler<T> handler, ILog logger)
        {
            _pollingMessageReceiver = pollingMessageReceiver;
            _handler = handler;
            _logger = logger;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var message = await _pollingMessageReceiver.ReceiveAsAsync<T>();

                if (message == null) continue;

                try
                {
                    _handler.Handle(message.Content);
                }
                catch (Exception ex)
                {
                    _logger.Fatal(ex, "Could not process task message");
                    break; //Stop processing anymore messages as this failure needs to be investigated
                }
            }
        }
    }
}
