using System.Threading.Tasks;
using MediatR;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Exceptions;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.Application.Queries.GetTask
{
    public class GetTaskRequestHandler : IAsyncRequestHandler<GetTaskRequest, GetTaskResponse>
    {
        private readonly ITaskRepository _repository;
        private readonly IValidator<GetTaskRequest> _validator;
        private readonly ILog _logger;

        public GetTaskRequestHandler(ITaskRepository repository, IValidator<GetTaskRequest> validator, ILog logger)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<GetTaskResponse> Handle(GetTaskRequest message)
        {
            var validationResult = _validator.Validate(message);

            if (!validationResult.IsValid())
            {
                throw new InvalidRequestException(validationResult.ValidationDictionary);
            }

            var task = await _repository.GetTask(message.OwnerId, message.Type);

            if (task != null)
            {
                _logger.Info($"Retrieved task for owner {message.OwnerId} of type {message.Type}");
            }
            else
            {
                _logger.Info($"Unable to retrieved a task for owner {message.OwnerId} of type {message.Type}");
            }

            return new GetTaskResponse
            {
                Task = task
            };
        }
    }
}
