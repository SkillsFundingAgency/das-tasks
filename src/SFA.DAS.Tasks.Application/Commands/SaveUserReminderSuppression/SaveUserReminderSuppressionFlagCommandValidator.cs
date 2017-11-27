using System;
using System.Threading.Tasks;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Application.Commands.SaveUserReminderSuppression
{
    public class SaveUserReminderSuppressionFlagCommandValidator : IValidator<SaveUserReminderSuppressionFlagCommand>
    {
        public ValidationResult Validate(SaveUserReminderSuppressionFlagCommand request)
        {
            var validationResult = new ValidationResult();

            if(!Enum.TryParse(request.TaskType, out TaskType type))
            {
                validationResult.AddError(nameof(request.TaskType), "Task type value is not supported");
            }

            if (string.IsNullOrEmpty(request.EmployerAccountId))
            {
                validationResult.AddError(nameof(request.EmployerAccountId), "Account ID cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(request.UserId))
            {
                validationResult.AddError(nameof(request.UserId), "User ID cannot be null or empty.");
            }

            return validationResult;
        }

        public Task<ValidationResult> ValidateAsync(SaveUserReminderSuppressionFlagCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
