using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Tasks.Application.Exceptions;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.Application.Queries.GetTasksByOwnerId
{
    public class GetTasksByOwnerIdHandler : IAsyncRequestHandler<GetTasksByOwnerIdRequest, GetTasksByOwnerIdResponse>
    {
        private readonly ITaskRepository _repository;
        private readonly IValidator<GetTasksByOwnerIdRequest> _validator;

        public GetTasksByOwnerIdHandler(ITaskRepository repository, IValidator<GetTasksByOwnerIdRequest> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<GetTasksByOwnerIdResponse> Handle(GetTasksByOwnerIdRequest message)
        {
            var validationResult = _validator.Validate(message);

            if (!validationResult.IsValid())
            {
                throw new InvalidRequestException(validationResult.ValidationDictionary);
            }

            var tasks = await _repository.GetTasks(message.OwnerId);

            var monthlyReminderTasks = await _repository.GetMonthlyReminderTasks(message.OwnerId);

            if (!string.IsNullOrEmpty(message.UserId))
            {
                var supressedTaskTypes = await _repository.GetUserTaskSuppressions(message.UserId, message.OwnerId);

                monthlyReminderTasks = monthlyReminderTasks.Where(t => !supressedTaskTypes.Contains(t.Type));
            }

            return new GetTasksByOwnerIdResponse {Tasks = tasks.Concat(monthlyReminderTasks)};
        }
    }
}
