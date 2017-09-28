using System.Threading.Tasks;

namespace SFA.DAS.Tasks.Application.Validation
{
    public interface IValidator<T>
    {
        ValidationResult Validate(T item);
    }
}