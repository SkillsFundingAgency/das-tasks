using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Tasks.Application.Exceptions;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.Application.Queries.GetTasksByEmployerAccountId
{
    public class GetTasksByEmployerAccountIdHandler : IAsyncRequestHandler<GetTasksByEmployerAccountIdRequest, GetTasksByEmployerAccountIdResponse>
    {
        private readonly ITaskRepository _repository;
        private readonly IValidator<GetTasksByEmployerAccountIdRequest> _validator;

        public GetTasksByEmployerAccountIdHandler(ITaskRepository repository, IValidator<GetTasksByEmployerAccountIdRequest> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<GetTasksByEmployerAccountIdResponse> Handle(GetTasksByEmployerAccountIdRequest message)
        {
            var validationResult = _validator.Validate(message);

            if (!validationResult.IsValid())
            {
                throw new InvalidRequestException(validationResult.ValidationDictionary);
            }

            var tasks = await _repository.GetTasks(message.EmployerAccountId);

            var monthlyReminderTasks = await _repository.GetMonthlyReminderTasks(message.EmployerAccountId);

            if (!string.IsNullOrEmpty(message.UserId))
            {
                var supressedTaskTypes = await _repository.GetUserTaskSuppressions(message.UserId, message.EmployerAccountId);

                monthlyReminderTasks = monthlyReminderTasks.Where(t => !supressedTaskTypes.Contains(t.Type));
            }

            return new GetTasksByEmployerAccountIdResponse {Tasks = tasks.Concat(monthlyReminderTasks)};
        }
    }
}
