﻿using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.AzureServiceBus.Attributes;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Worker.MessageProcessors
{
    [TopicSubscription("Task_SignedEmployerAgreementProcessor")]
    public class SignedEmployerAgreementMessageProcessor : MessageProcessor<AgreementSignedMessage>
    {
        private readonly ILog _logger;
        private readonly IMediator _mediator;

        public SignedEmployerAgreementMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog logger, IMediator mediator) : base(subscriberFactory, logger)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        protected override async Task ProcessMessage(AgreementSignedMessage message)
        {
            _logger.Debug($"Saving agreement signed task for account id {message.AccountId}, " +
                          $"agreement id {message.AgreementId} and legal enityt id {message.LegalEntityId}");
            await _mediator.SendAsync(new SaveTaskCommand
            {
                OwnerId = message.AccountId.ToString(),
                Type = TaskType.AgreementToSign,
                TaskCompleted = true
            });
        }
    }
}
