using System.Threading.Tasks;
using SFA.DAS.Tasks.Application.Validation;

namespace SFA.DAS.Tasks.Application.Queries.GetTasksByEmployerAccountId
{
    public class GetTasksByEmployerAccountIdValidator : IValidator<GetTasksByEmployerAccountIdRequest>
    {
        public ValidationResult Validate(GetTasksByEmployerAccountIdRequest request)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrEmpty(request.EmployerAccountId))
            {
                validationResult.AddError(nameof(request.EmployerAccountId), "Cannot get tasks when employer account ID is not given.");
            }

            return validationResult;
        }

        public Task<ValidationResult> ValidateAsync(GetTasksByEmployerAccountIdRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
