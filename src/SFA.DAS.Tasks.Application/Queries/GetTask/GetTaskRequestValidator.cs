using System.Threading.Tasks;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Application.Queries.GetTask
{
    public class GetTaskRequestValidator : IValidator<GetTaskRequest>
    {
        public ValidationResult Validate(GetTaskRequest request)
        {
            var validationResults = new ValidationResult();

            if (string.IsNullOrEmpty(request.EmployerAccountId))
            {
                validationResults.AddError(nameof(request.EmployerAccountId), "Cannot get task when employer account ID is not given.");
            }

            if(request.Type == TaskType.None)
            {
                validationResults.AddError(nameof(request.Type), "Cannot get task when type is not given.");
            }

            return validationResults;
        }

        public Task<ValidationResult> ValidateAsync(GetTaskRequest item)
        {
            throw new System.NotImplementedException();
        }
    }
}
