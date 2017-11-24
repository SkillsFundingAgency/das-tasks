using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Exceptions;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Domain.Models;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.Application.Commands.SaveMonthlyUserDismiss
{
    public class SaveUserReminderSupressionFlagCommandHandler : IAsyncRequestHandler<SaveUserReminderSupressionFlagCommand, SaveUserReminderSupressionFlagCommandResponse>
    {
        private readonly ITaskRepository _repository;
        private readonly ILog _logger;
        private readonly IValidator<SaveUserReminderSupressionFlagCommand> _validator;

        public SaveUserReminderSupressionFlagCommandHandler(ITaskRepository repository, ILog logger, IValidator<SaveUserReminderSupressionFlagCommand> validator)
        {
            _repository = repository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<SaveUserReminderSupressionFlagCommandResponse> Handle(SaveUserReminderSupressionFlagCommand command)
        {
            var validationResults = _validator.Validate(command);

            if (!validationResults.IsValid())
            {
                throw new InvalidRequestException(validationResults.ValidationDictionary);
            }

            Enum.TryParse(command.TaskType, out TaskType type);

            await _repository.SaveUserReminderSupression(new UserReminderSupressionFlag
            {
                UserId = command.UserId,
                AccountId = command.AccountId,
                ReminderType = type
            });

            return new SaveUserReminderSupressionFlagCommandResponse();
        }
    }
}
