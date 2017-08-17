using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.Attributes;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.Domain.Enums;

namespace SFA.DAS.Tasks.Worker.MessageProcessors
{
    public class CreatedEmployerAgreementMessageProcessor : MessageProcessor<AgreementCreatedMessage>
    {
        private readonly IMediator _mediator;

        [QueueName]
        public string agreement_created_messages { get; set; }

        public CreatedEmployerAgreementMessageProcessor(IPollingMessageReceiver pollingMessageReceiver, ILog logger, IMediator mediator) : base(pollingMessageReceiver, logger)
        {
            _mediator = mediator;
        }
        
        protected override async Task ProcessMessage(AgreementCreatedMessage message)
        {
            await _mediator.SendAsync(new SaveTaskCommand
            {
                OwnerId = message.AccountId.ToString(),
                Type = TaskType.AgreementToSign,
                TaskCompleted = false
            });
        }
    }
}
