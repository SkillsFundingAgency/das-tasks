using System.Threading.Tasks;
using SFA.DAS.Tasks.Application.Validation;

namespace SFA.DAS.Tasks.Application.Queries.GetTasksByOwnerId
{
    public class GetTasksByOwnerIdValidator : IValidator<GetTasksByOwnerIdRequest>
    {
        public ValidationResult Validate(GetTasksByOwnerIdRequest item)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrEmpty(item.OwnerId))
            {
                validationResult.AddError(nameof(item.OwnerId), "Cannot get tasks when owner ID is not given.");
            }

            return validationResult;
        }

        public Task<ValidationResult> ValidateAsync(GetTasksByOwnerIdRequest item)
        {
            throw new System.NotImplementedException();
        }
    }
}
