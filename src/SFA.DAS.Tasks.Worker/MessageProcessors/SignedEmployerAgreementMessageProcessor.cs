﻿using System;
using System.Threading.Tasks;
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
    [TopicSubscription("Task_SignedEmployerAgreementProcessor")]
    public class SignedEmployerAgreementMessageProcessor : MessageProcessor2<AgreementSignedMessage>
    {
        private readonly ILog _logger;
        private readonly IMediator _mediator;

        public SignedEmployerAgreementMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog logger, IMessageContextProvider messageContextProvider, IMediator mediator) 
            : base(subscriberFactory, logger, messageContextProvider)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        protected override async Task ProcessMessage(AgreementSignedMessage message)
        {
            await RemoveAgreementToSignTask(message);

            if (message.CohortCreated) return;

            await CreateAddApprenticesTask(message);
        }

        private async Task CreateAddApprenticesTask(AgreementSignedMessage message)
        {
            try
            {
                _logger.Debug($"Saving 'add apprentices' task for account id {message.AccountId} as the agreement " +
                              $"(ID: {message.AgreementId}) has been signed");

                await _mediator.SendAsync(new SaveTaskCommand
                {
                    EmployerAccountId = message.AccountId.ToString(),
                    Type = TaskType.AddApprentices,
                    TaskCompleted = false
                });
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Failed to create add apprentices task [Account ID: {message.AccountId}, " +
                                 $"Agreement ID: {message.AgreementId}, Legal Entity ID: {message.LegalEntityId}");
            }
        }

        private async Task RemoveAgreementToSignTask(AgreementSignedMessage message)
        {
            try
            {
                _logger.Debug($"Removing 'agreement to sign' task from account (ID: {message.AccountId}) as the " +
                              $"agreement (ID: {message.AgreementId}) has been signed");

                await _mediator.SendAsync(new SaveTaskCommand
                {
                    EmployerAccountId = message.AccountId.ToString(),
                    Type = TaskType.AgreementToSign,
                    TaskCompleted = true
                });
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Failed to complete agreement to sign task [Account ID: {message.AccountId}, " +
                                 $"Agreement ID: {message.AgreementId}, Legal Entity ID: {message.LegalEntityId}");
            }
        }
    }
}
