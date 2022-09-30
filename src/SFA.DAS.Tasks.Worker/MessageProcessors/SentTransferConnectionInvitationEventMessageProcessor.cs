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
    [TopicSubscription("Task_SentTransferConnectionInvitiationProcessor")]
    public class SentTransferConnectionInvitationEventMessageProcessor : MessageProcessor2<SentTransferConnectionInvitationEvent>
    {
        private readonly ILog _logger;
        private readonly IMediator _mediator;

        public SentTransferConnectionInvitationEventMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog log, IMessageContextProvider messageContextProvider, IMediator mediator) 
            : base(subscriberFactory, log, messageContextProvider)
        {
            _logger = log;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(SentTransferConnectionInvitationEvent message)
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
