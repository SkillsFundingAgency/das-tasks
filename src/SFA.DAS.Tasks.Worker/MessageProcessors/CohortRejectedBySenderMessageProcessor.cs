using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.AzureServiceBus.Attributes;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Infrastructure.Attributes;

namespace SFA.DAS.Tasks.Worker.MessageProcessors
{
    [ServiceBusConnectionString("Commitments")]
    [TopicSubscription("Task_CohortRejectedByTransferSender")]
    public class CohortRejectedBySenderMessageProcessor : MessageProcessor<CohortRejectedByTransferSender>
    {
        private readonly ILog _log;
        private readonly IMediator _mediator;

        public CohortRejectedBySenderMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog log, IMediator mediator) 
            : base(subscriberFactory, log)
        {
            _log = log;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(CohortRejectedByTransferSender message)
        {
            _log.Debug($"Cohort approval rejected by sender. Completing 'TransferRequestReceived' task for account id {message.SendingEmployerAccountId}, " +
                       $"commitment id {message.CommitmentId}");

            await _mediator.SendAsync(new SaveTaskCommand
            {
                EmployerAccountId = message.SendingEmployerAccountId.ToString(),
                Type = TaskType.TransferRequestReceived,
                TaskCompleted = true
            });
        }
    }
}
