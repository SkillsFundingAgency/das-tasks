using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.AzureServiceBus.Attributes;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Worker.MessageProcessors
{
    [TopicSubscription("Task_CohortApprovedMessageProcessor")]
    public class CohortApprovalRequestedMessageProcessor: MessageProcessor<CohortApprovalRequestedByProvider>
    {
        private readonly ILog _log;
        private readonly IMediator _mediator;

        public CohortApprovalRequestedMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog log, IMediator mediator) 
            : base(subscriberFactory, log)
        {
            _log = log;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(CohortApprovalRequestedByProvider message)
        {
            _log.Debug($"Saving 'apprentice changes to review' task for account id {message.AccountId}, " +
                       $"commitment id {message.CommitmentId} and provider id {message.ProviderId}");

            await _mediator.SendAsync(new SaveTaskCommand
            {
                EmployerAccountId = message.AccountId.ToString(),
                Type = TaskType.CohortRequestReadyForApproval,
                TaskCompleted = false
            });
        }
    }
}
