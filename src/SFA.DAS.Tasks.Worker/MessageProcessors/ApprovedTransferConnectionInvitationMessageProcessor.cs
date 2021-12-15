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
    [TopicSubscription("Task_ApprovedTransferConnectionInvitation")]

    public class ApprovedTransferConnectionInvitationMessageProcessor : MessageProcessor2<ApprovedTransferConnectionInvitationEvent>
    {
        private readonly ILog _logger;
        private readonly IMediator _mediator;

        public ApprovedTransferConnectionInvitationMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog logger, IMediator mediator, IMessageContextProvider messageContextProvider)
            : base(subscriberFactory, logger, messageContextProvider)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(ApprovedTransferConnectionInvitationEvent message)
        {
            _logger.Debug($"Connection request approved. Completing '{nameof(TaskType.ReviewConnectionRequest)}' task for sender account id {message.SenderAccountId} / receiver account {message.ReceiverAccountId}");

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
