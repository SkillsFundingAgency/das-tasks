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
    [TopicSubscription("Task_CohortApprovalRequested")]
    public class CohortApprovalRequestedMessageProcessor: MessageProcessor2<CohortApprovalRequestedByProvider>
    {
        private readonly ILog _log;
        private readonly IMediator _mediator;

        public CohortApprovalRequestedMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog log, IMediator mediator, IMessageContextProvider messageContextProvider) 
            : base(subscriberFactory, log, messageContextProvider)
        {
            _log = log;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(CohortApprovalRequestedByProvider message)
        {
            _log.Debug($"Cohort approval requested by provider. Saving 'cohort approval requested' task for account id {message.AccountId}, " +
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
