using System;
using System.Threading.Tasks;
using SFA.DAS.Tasks.Application.Validation;

namespace SFA.DAS.Tasks.Application.Commands.SaveMonthlyUserDismiss
{
    public class SaveMonthlyReminderDismissCommadValidator : IValidator<SaveMonthlyReminderDismissCommand>
    {
        public ValidationResult Validate(SaveMonthlyReminderDismissCommand item)
        {
            throw new NotImplementedException();
        }

        public Task<ValidationResult> ValidateAsync(SaveMonthlyReminderDismissCommand item)
        {
            throw new NotImplementedException();
        }
    }
}
