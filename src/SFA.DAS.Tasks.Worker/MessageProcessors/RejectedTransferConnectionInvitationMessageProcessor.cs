using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.AzureServiceBus.Attributes;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Infrastructure.Attributes;
using SFA.DAS.EmployerFinance.Events.Messages;

namespace SFA.DAS.Tasks.Worker.MessageProcessors
{
    [ServiceBusConnectionString("ManageApprenticeships")]
    [TopicSubscription("Task_RejectedTransferConnectionInvitation")]

    public class RejectedTransferConnectionInvitationMessageProcessor : MessageProcessor2<RejectedTransferConnectionInvitationEvent>
    {
        private readonly ILog _logger;
        private readonly IMediator _mediator;

        public RejectedTransferConnectionInvitationMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog logger, IMessageContextProvider messageContextProvider, IMediator mediator)
            : base(subscriberFactory, logger, messageContextProvider)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(RejectedTransferConnectionInvitationEvent message)
        {
            _logger.Debug($"Connection request rejected. Completing '{nameof(TaskType.ReviewConnectionRequest)}' task for sender account id {message.SenderAccountId} / receiver account {message.ReceiverAccountId}");

            await _mediator.SendAsync(new SaveTaskCommand
            {
                EmployerAccountId = message.ReceiverAccountId.ToString(),
                Type = TaskType.ReviewConnectionRequest,
                TaskCompleted = true,
                CompleteAllTasks = false
            });
        }
    }
}
