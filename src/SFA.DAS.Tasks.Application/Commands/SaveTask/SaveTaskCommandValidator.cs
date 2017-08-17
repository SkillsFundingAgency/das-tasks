using System.Threading.Tasks;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.Domain.Enums;

namespace SFA.DAS.Tasks.Application.Commands.SaveTask
{
    public class SaveTaskCommandValidator : IValidator<SaveTaskCommand>
    {
        public ValidationResult Validate(SaveTaskCommand item)
        {
            var validationResults = new ValidationResult();

            if (string.IsNullOrEmpty(item.OwnerId))
            {
                validationResults.AddError(nameof(item.OwnerId), "Cannot save task when owner ID is not given.");
            }

            if (item.Type == TaskType.None)
            {
                validationResults.AddError(nameof(item.Type), "Cannot save task when task type is not given.");
            }

            return validationResults;
        }

        public Task<ValidationResult> ValidateAsync(SaveTaskCommand item)
        {
            throw new System.NotImplementedException();
        }
    }
}
