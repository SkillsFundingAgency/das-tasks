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
    [TopicSubscription("Task_ApprenticeshipUpdateCancelled")]
    public class ApprenticeshipUpdateCancelledMessageProcessor : MessageProcessor2<ApprenticeshipUpdateCancelled>
    {
        private readonly ILog _logger;
        private readonly IMediator _mediator;

        public ApprenticeshipUpdateCancelledMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog logger, IMediator mediator, IMessageContextProvider messageContextProvider) 
            : base(subscriberFactory, logger, messageContextProvider)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(ApprenticeshipUpdateCancelled message)
        {
            _logger.Debug($"Apprenticeship updated cancelled. Completing 'apprentice changes to review' task for account id {message.AccountId}, " +
                          $"apprentice id {message.ApprenticeshipId} and provider id {message.ProviderId}");

            await _mediator.SendAsync(new SaveTaskCommand
            {
                EmployerAccountId = message.AccountId.ToString(),
                Type = TaskType.ApprenticeChangesToReview,
                TaskCompleted = true
            });
        }
    }
}
