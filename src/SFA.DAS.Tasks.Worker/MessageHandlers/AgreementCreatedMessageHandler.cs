using MediatR;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.Application.Queries.GetTask;
using SFA.DAS.Tasks.Domain.Enums;
using SFA.DAS.Tasks.Domain.Models;

namespace SFA.DAS.Tasks.Worker.MessageHandlers
{
    public class AgreementCreatedMessageHandler : IMessageHandler<AgreementCreatedMessage>
    {
        private readonly IMediator _mediator;

        public AgreementCreatedMessageHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async void Handle(AgreementCreatedMessage message)
        {
            var response = await _mediator.SendAsync(new GetTaskRequest
                {
                    OwnerId = message.AccountId.ToString(),
                    Type = TaskType.AgreementToSign
                });

            var task = response?.Task ?? new DasTask
            {
                OwnerId = message.AccountId.ToString(),
                Type = TaskType.AgreementToSign
            };

            task.ItemsDueCount++;

            await _mediator.SendAsync(new SaveTaskCommand{Task = task});
        }
    }
}
