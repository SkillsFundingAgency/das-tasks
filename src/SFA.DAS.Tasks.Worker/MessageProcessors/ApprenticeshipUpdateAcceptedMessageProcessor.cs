using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Worker.MessageProcessors
{
    public class ApprenticeshipUpdateAcceptedMessageProcessor : MessageProcessor<ApprenticeshipUpdateAccepted>
    {
        private readonly ILog _logger;
        private readonly IMediator _mediator;

        public ApprenticeshipUpdateAcceptedMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog logger, IMediator mediator) 
            : base(subscriberFactory, logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(ApprenticeshipUpdateAccepted message)
        {
            _logger.Debug($"Completing 'apprentice changes to review' task for account id {message.AccountId}, " +
                       $"apprentice id {message.ApprenticeshipId} and provider id {message.ProviderId}");

            await _mediator.SendAsync(new SaveTaskCommand
            {
                OwnerId = message.AccountId.ToString(),
                Type = TaskType.ApprenticeChangesToReview,
                TaskCompleted = true
            });
        }
    }
}
