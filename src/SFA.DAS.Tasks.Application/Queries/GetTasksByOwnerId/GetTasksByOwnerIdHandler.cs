using System.Threading.Tasks;
using MediatR;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Exceptions;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.Application.Queries.GetTasksByOwnerId
{
    public class GetTasksByOwnerIdHandler : IAsyncRequestHandler<GetTasksByOwnerIdRequest, GetTasksByOwnerIdResponse>
    {
        private readonly ITaskRepository _repository;
        private readonly IValidator<GetTasksByOwnerIdRequest> _validator;
        private readonly ILog _logger;

        public GetTasksByOwnerIdHandler(ITaskRepository repository, IValidator<GetTasksByOwnerIdRequest> validator, ILog logger)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<GetTasksByOwnerIdResponse> Handle(GetTasksByOwnerIdRequest message)
        {
            var validationResult = _validator.Validate(message);

            if (!validationResult.IsValid())
            {
                throw new InvalidRequestException(validationResult.ValidationDictionary);
            }

            _logger.Info($"Getting tasks for owner {message.OwnerId}");

            var result = await _repository.GetTasks(message.OwnerId);

            return new GetTasksByOwnerIdResponse {Tasks = result};
        }
    }
}
