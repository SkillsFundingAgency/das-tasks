using System;
using System.Threading.Tasks;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Application.Commands.SaveUserReminderSuppression
{
    public class SaveUserReminderSuppressionFlagCommandValidator : IValidator<SaveUserReminderSuppressionFlagCommand>
    {
        public ValidationResult Validate(SaveUserReminderSuppressionFlagCommand command)
        {
            var validationResult = new ValidationResult();

            if(!Enum.TryParse(command.TaskType, out TaskType type))
            {
                validationResult.AddError(nameof(command.TaskType), "Task type value is not supported");
            }

            if (string.IsNullOrEmpty(command.AccountId))
            {
                validationResult.AddError(nameof(command.AccountId), "Account ID cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(command.UserId))
            {
                validationResult.AddError(nameof(command.UserId), "User ID cannot be null or empty.");
            }

            return validationResult;
        }

        public Task<ValidationResult> ValidateAsync(SaveUserReminderSuppressionFlagCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
