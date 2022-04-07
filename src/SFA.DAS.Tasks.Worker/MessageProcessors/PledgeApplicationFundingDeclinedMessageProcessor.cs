using MediatR;
using SFA.DAS.LevyTransferMatching.Messages.Legacy;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Tasks.Worker.MessageProcessors
{
    public class PledgeApplicationFundingDeclinedMessageProcessor : MessageProcessor<PledgeApplicationFundingDeclined>
    {
        private readonly ILog _logger;
        private readonly IMediator _mediator;

        public PledgeApplicationFundingDeclinedMessageProcessor(IMessageSubscriberFactory subscriberFactory, ILog logger, IMediator mediator)
            : base(subscriberFactory, logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected override async Task ProcessMessage(PledgeApplicationFundingDeclined message)
        {
            _logger.Debug($"PledgeApplicationFundingDeclined id {message.ApplicationId}");

            await _mediator.SendAsync(new SaveTaskCommand
            {
                EmployerAccountId = message.AccountId.ToString(),
                Type = TaskType.PledgeApplicationForAcceptance,
                TaskCompleted = true
            });
        }
    }
}
