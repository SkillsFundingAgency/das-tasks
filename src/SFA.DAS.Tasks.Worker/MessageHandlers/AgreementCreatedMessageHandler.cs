using System;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Tasks.Domain.Repositories;


namespace SFA.DAS.Tasks.Worker.MessageHandlers
{
    public class AgreementCreatedMessageHandler : IMessageHandler<AgreementCreatedMessage>
    {
        private readonly ITaskRepository _repository;

        public AgreementCreatedMessageHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public void Handle(AgreementCreatedMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
