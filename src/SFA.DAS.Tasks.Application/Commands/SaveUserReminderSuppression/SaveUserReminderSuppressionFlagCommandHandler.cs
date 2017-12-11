using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Exceptions;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Domain.Models;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.Application.Commands.SaveUserReminderSuppression
{
    public class SaveUserReminderSuppressionFlagCommandHandler : IAsyncRequestHandler<SaveUserReminderSuppressionFlagCommand, SaveUserReminderSuppressionFlagCommandResponse>
    {
        private readonly ITaskRepository _repository;
        private readonly ILog _logger;
        private readonly IValidator<SaveUserReminderSuppressionFlagCommand> _validator;

        public SaveUserReminderSuppressionFlagCommandHandler(ITaskRepository repository, ILog logger, IValidator<SaveUserReminderSuppressionFlagCommand> validator)
        {
            _repository = repository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<SaveUserReminderSuppressionFlagCommandResponse> Handle(SaveUserReminderSuppressionFlagCommand command)
        {
            var validationResults = _validator.Validate(command);

            if (!validationResults.IsValid())
            {
                throw new InvalidRequestException(validationResults.ValidationDictionary);
            }

            Enum.TryParse(command.TaskType, out TaskType type);

            await _repository.SaveUserReminderSuppression(new UserReminderSuppressionFlag
            {
                UserId = command.UserId,
                EmployerAccountId = command.EmployerAccountId,
                ReminderType = type
            });

            return new SaveUserReminderSuppressionFlagCommandResponse();
        }
    }
}
