using System.Threading.Tasks;

namespace SFA.DAS.Tasks.Application.Validation
{
    public interface IValidator<in T>
    {
        ValidationResult Validate(T request);

        Task<ValidationResult> ValidateAsync(T item);
    }
}