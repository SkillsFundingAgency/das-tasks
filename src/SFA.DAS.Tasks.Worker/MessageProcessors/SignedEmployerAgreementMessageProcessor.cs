using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Messaging;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Worker.MessageProcessors
{
    public class SignedEmployerAgreementMessageProcessor : MessageProcessor<AgreementSignedMessage>
    {
        private readonly IMediator _mediator;

        public SignedEmployerAgreementMessageProcessor(IPollingMessageReceiver pollingMessageReceiver, ILog logger, IMediator mediator) : base(pollingMessageReceiver, logger)
        {
            _mediator = mediator;
        }
        
        protected override async Task ProcessMessage(AgreementSignedMessage message)
        {
            await _mediator.SendAsync(new SaveTaskCommand
            {
                OwnerId = message.AccountId.ToString(),
                Type = TaskType.AgreementToSign,
                TaskCompleted = true
            });
        }
    }
}
