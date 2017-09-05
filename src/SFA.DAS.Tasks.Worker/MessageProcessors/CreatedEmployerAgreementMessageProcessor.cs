using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Messaging;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Worker.MessageProcessors
{
    public class CreatedEmployerAgreementMessageProcessor : MessageProcessor<AgreementCreatedMessage>
    {
        private readonly IMediator _mediator;

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
