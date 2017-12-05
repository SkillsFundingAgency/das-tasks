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
    [TopicSubscription("Task_CohortCreatedMessageProcessor")]
    public class CohortCreatedMessageProcessor : MessageProcessor<CohortCreated>
    {
        private readonly ILog _logger;
        private readonly IMediator _mediator;

        public CohortCreatedMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog log, IMediator mediator) 
            : base(subscriberFactory, log)
        {
            _logger = log;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(CohortCreated message)
        {
            _logger.Debug($"Completing add aprrentices task for account id {message.AccountId}");

            await _mediator.SendAsync(new SaveTaskCommand
            {
                OwnerId = message.AccountId.ToString(),
                Type = TaskType.AddApprentices,
                TaskCompleted = true
            });
        }
    }
}
