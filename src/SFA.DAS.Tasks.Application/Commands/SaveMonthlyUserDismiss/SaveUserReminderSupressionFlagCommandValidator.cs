using System;
using System.Threading.Tasks;
using SFA.DAS.Tasks.Application.Validation;

namespace SFA.DAS.Tasks.Application.Commands.SaveMonthlyUserDismiss
{
    public class SaveUserReminderSupressionFlagCommandValidator : IValidator<SaveUserReminderSupressionFlagCommand>
    {
        public ValidationResult Validate(SaveUserReminderSupressionFlagCommand item)
        {
            throw new NotImplementedException();
        }

        public Task<ValidationResult> ValidateAsync(SaveUserReminderSupressionFlagCommand item)
        {
            throw new NotImplementedException();
        }
    }
}
