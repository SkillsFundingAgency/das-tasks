using System.Threading.Tasks;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Application.Queries.GetTask
{
    public class GetTaskRequestValidator : IValidator<GetTaskRequest>
    {
        public ValidationResult Validate(GetTaskRequest item)
        {
            var validationResults = new ValidationResult();

            if (string.IsNullOrEmpty(item.OwnerId))
            {
                validationResults.AddError(nameof(item.OwnerId), "Cannot get task when owner ID is not given.");
            }

            if(item.Type == TaskType.None)
            {
                validationResults.AddError(nameof(item.Type), "Cannot get task when type is not given.");
            }

            return validationResults;
        }

        public Task<ValidationResult> ValidateAsync(GetTaskRequest item)
        {
            throw new System.NotImplementedException();
        }
    }
}
