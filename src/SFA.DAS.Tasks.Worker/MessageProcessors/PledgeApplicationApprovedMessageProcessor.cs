using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Messages.Legacy;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.AzureServiceBus.Attributes;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.Infrastructure.Attributes;

namespace SFA.DAS.Tasks.Worker.MessageProcessors
{
    [ServiceBusConnectionString("SharedServiceBus")]
    [TopicSubscription("Task_PledgeApplicationApproved")]
    public class PledgeApplicationApprovedMessageProcessor : MessageProcessor<PledgeApplicationApproved>
    {
        private readonly ILog _logger;
        private readonly IMediator _mediator;

        public PledgeApplicationApprovedMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog logger, IMediator mediator)
            : base(subscriberFactory, logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(PledgeApplicationApproved message)
        {
            _logger.Debug($"PledgeApplicationApproved id {message.ApplicationId}");

            await _mediator.SendAsync(new SaveTaskCommand
            {
                EmployerAccountId = message.TransferSenderId.ToString(),
                Type = TaskType.PledgeApplicationForReview,
                TaskCompleted = true
            });
        }
    }
}