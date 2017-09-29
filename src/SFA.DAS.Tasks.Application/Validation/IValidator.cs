using System.Threading.Tasks;

namespace SFA.DAS.Tasks.Application.Validation
{
    public interface IValidator<in T>
    {
        ValidationResult Validate(T item);

        Task<ValidationResult> ValidateAsync(T item);
    }
}