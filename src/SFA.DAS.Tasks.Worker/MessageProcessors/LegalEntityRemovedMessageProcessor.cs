﻿using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.AzureServiceBus.Attributes;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Infrastructure.Attributes;

namespace SFA.DAS.Tasks.Worker.MessageProcessors
{
    [ServiceBusConnectionString("ManageApprenticeships")]
    [TopicSubscription("Task_LegalEntityRemovedProcessor")]
    public class LegalEntityRemovedMessageProcessor : MessageProcessor2<LegalEntityRemovedMessage>
    {
        private readonly ILog _logger;
        private readonly IMediator _mediator;

        public LegalEntityRemovedMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog logger, IMessageContextProvider messageContextProvider, IMediator mediator) 
            : base(subscriberFactory, logger, messageContextProvider)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(LegalEntityRemovedMessage message)
        {
            _logger.Debug($"Legal entity is being removed (ID:{message?.LegalEntityId})");

            if (message?.AgreementSigned != null && !message.AgreementSigned)
            {
                _logger.Debug("Legal entity's agreement task is also being removed as it is incomplete");
                await _mediator.SendAsync(new SaveTaskCommand
                {
                    EmployerAccountId = message.AccountId.ToString(),
                    Type = TaskType.AgreementToSign,
                    TaskCompleted = true
                });
            }
        }
    }
}
