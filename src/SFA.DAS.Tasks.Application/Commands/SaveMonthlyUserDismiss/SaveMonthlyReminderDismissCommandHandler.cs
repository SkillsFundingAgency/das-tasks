using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Exceptions;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.Application.Commands.SaveMonthlyUserDismiss
{
    public class SaveMonthlyReminderDismissCommandHandler : IAsyncRequestHandler<SaveMonthlyReminderDismissCommand, SaveMonthlyReminderDismissCommandResponse>
    {
        private readonly ITaskRepository _repository;
        private readonly ILog _logger;
        private readonly IValidator<SaveMonthlyReminderDismissCommand> _validator;

        public SaveMonthlyReminderDismissCommandHandler(ITaskRepository repository, ILog logger, IValidator<SaveMonthlyReminderDismissCommand> validator)
        {
            _repository = repository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<SaveMonthlyReminderDismissCommandResponse> Handle(SaveMonthlyReminderDismissCommand command)
        {
            var validationResults = _validator.Validate(command);

            if (!validationResults.IsValid())
            {
                throw new InvalidRequestException(validationResults.ValidationDictionary);
            }

            Enum.TryParse(command.TaskType, out TaskType type);

            await _repository.SaveMonthlyReminderDismiss(command.UserId, command.AccountId, type);

            return new SaveMonthlyReminderDismissCommandResponse();
        }
    }
}
