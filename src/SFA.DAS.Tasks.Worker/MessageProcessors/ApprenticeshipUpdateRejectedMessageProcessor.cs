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
    [TopicSubscription("Task_ApprenticeshipUpdateRejected")]
    public class ApprenticeshipUpdateRejectedMessageProcessor : MessageProcessor2<ApprenticeshipUpdateRejected>
    {
        private readonly ILog _logger;
        private readonly IMediator _mediator;

        public ApprenticeshipUpdateRejectedMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog logger, IMessageContextProvider messageContextProvider, IMediator mediator) 
            : base(subscriberFactory, logger, messageContextProvider)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(ApprenticeshipUpdateRejected message)
        {
            _logger.Debug($"Apprenticeship updated rejected. Completing 'apprentice changes to review' task for account id {message.AccountId}, " +
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
