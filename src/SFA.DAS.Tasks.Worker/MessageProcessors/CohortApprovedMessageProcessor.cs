﻿using System.Threading.Tasks;
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
    public class CohortApprovedMessageProcessor : MessageProcessor<CohortApprovedByEmployer>
    {
        private readonly ILog _logger;
        private readonly IMediator _mediator;

        public CohortApprovedMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog logger, IMediator mediator) 
            : base(subscriberFactory, logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(CohortApprovedByEmployer message)
        {
            _logger.Debug($"Completing 'cohort approval requested' task for account id {message.AccountId}, " +
                       $"commitment id {message.CommitmentId} and provider id {message.ProviderId}");

            await _mediator.SendAsync(new SaveTaskCommand
            {
                OwnerId = message.AccountId.ToString(),
                Type = TaskType.CohortRequestReadyForApproval,
                TaskCompleted = true
            });
        }
    }
}
