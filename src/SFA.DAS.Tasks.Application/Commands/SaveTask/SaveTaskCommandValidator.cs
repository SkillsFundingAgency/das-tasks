using System.Threading.Tasks;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Application.Commands.SaveTask
{
    public class SaveTaskCommandValidator : IValidator<SaveTaskCommand>
    {
        public ValidationResult Validate(SaveTaskCommand request)
        {
            var validationResults = new ValidationResult();

            if (string.IsNullOrEmpty(request.EmployerAccountId))
            {
                validationResults.AddError(nameof(request.EmployerAccountId), "Cannot save task when employer account ID is not given.");
            }

            if (request.Type == TaskType.None)
            {
                validationResults.AddError(nameof(request.Type), "Cannot save task when task type is not given.");
            }

            return validationResults;
        }

        public Task<ValidationResult> ValidateAsync(SaveTaskCommand item)
        {
            throw new System.NotImplementedException();
        }
    }
}
