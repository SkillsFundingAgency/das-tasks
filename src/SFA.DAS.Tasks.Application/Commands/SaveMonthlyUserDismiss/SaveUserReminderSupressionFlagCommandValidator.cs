using System;
using System.Threading.Tasks;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Application.Commands.SaveMonthlyUserDismiss
{
    public class SaveUserReminderSupressionFlagCommandValidator : IValidator<SaveUserReminderSupressionFlagCommand>
    {
        public ValidationResult Validate(SaveUserReminderSupressionFlagCommand command)
        {
            var validationResult = new ValidationResult();

            if(!Enum.TryParse(command.TaskType, out TaskType type))
            {
                validationResult.AddError(nameof(command.TaskType), "Task type value is not supported");
            }

            if (command.AccountId <= 0)
            {
                validationResult.AddError(nameof(command.AccountId), "Account ID cannot be zero or less.");
            }

            if (command.UserId <= 0)
            {
                validationResult.AddError(nameof(command.UserId), "User ID cannot be zero or less.");
            }

            return validationResult;
        }

        public Task<ValidationResult> ValidateAsync(SaveUserReminderSupressionFlagCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
