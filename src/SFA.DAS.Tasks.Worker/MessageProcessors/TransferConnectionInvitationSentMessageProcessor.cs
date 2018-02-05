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
    [TopicSubscription("transfer_connection_invitation_sent")]
    public class TransferConnectionInvitationSentMessageProcessor : MessageProcessor<TransferConnectionInvitationSentMessage>
    {
        private readonly ILog _logger;
        private readonly IMediator _mediator;

        public TransferConnectionInvitationSentMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog log, IMediator mediator) 
            : base(subscriberFactory, log)
        {
            _logger = log;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(TransferConnectionInvitationSentMessage message)
        {
            _logger.Debug($"Connection request created. Completing '{nameof(TaskType.ReviewConnectionRequest)}' task for sender account id {message.SenderAccountId} / receiver account {message.ReceiverAccountId}");

            await _mediator.SendAsync(new SaveTaskCommand
            {
                EmployerAccountId = message.ReceiverAccountId.ToString(),
                Type = TaskType.ReviewConnectionRequest,
                TaskCompleted = false,
                CompleteAllTasks = false
            });
        }
    }
}
