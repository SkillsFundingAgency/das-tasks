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
    [TopicSubscription("Task_ApprenticeUpdateCreated")]
    public class ApprenticeshipUpdateCreatedMessageProcessor : MessageProcessor2<ApprenticeshipUpdateCreated>
    {
        private readonly ILog _log;
        private readonly IMediator _mediator;

        public ApprenticeshipUpdateCreatedMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog log, IMediator mediator) 
            : base(subscriberFactory, log, new MessageContextProvider())
        {
            _log = log;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(ApprenticeshipUpdateCreated message)
        {
            _log.Debug($"Apprenticeship updated created. Saving 'apprentice changes to review' task for account id {message.AccountId}, " +
                          $"apprentice id {message.ApprenticeshipId} and provider id {message.ProviderId}");

            await _mediator.SendAsync(new SaveTaskCommand
            {
                EmployerAccountId = message.AccountId.ToString(),
                Type = TaskType.ApprenticeChangesToReview,
                TaskCompleted = false
            });
        }
    }
}
