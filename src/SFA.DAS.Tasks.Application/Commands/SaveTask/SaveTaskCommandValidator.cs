using System.Threading.Tasks;
using SFA.DAS.Tasks.Application.Validation;

namespace SFA.DAS.Tasks.Application.Commands.SaveTask
{
    public class SaveTaskCommandValidator : IValidator<SaveTaskCommand>
    {
        public ValidationResult Validate(SaveTaskCommand item)
        {
            throw new System.NotImplementedException();
        }

        public Task<ValidationResult> ValidateAsync(SaveTaskCommand item)
        {
            throw new System.NotImplementedException();
        }
    }
}
