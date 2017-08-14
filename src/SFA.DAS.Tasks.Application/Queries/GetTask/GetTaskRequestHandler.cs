using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Tasks.Application.Exceptions;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.Application.Queries.GetTask
{
    public class GetTaskRequestHandler : IAsyncRequestHandler<GetTaskRequest, GetTaskResponse>
    {
        private readonly ITaskRepository _repository;
        private readonly IValidator<GetTaskRequest> _validator;

        public GetTaskRequestHandler(ITaskRepository repository, IValidator<GetTaskRequest> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<GetTaskResponse> Handle(GetTaskRequest message)
        {
            var validationResult = _validator.Validate(message);

            if (!validationResult.IsValid())
            {
                throw new InvalidRequestException(validationResult.ValidationDictionary);
            }

            var task = await _repository.GetTask(message.OwnerId, message.Type);

            return new GetTaskResponse
            {
                Task = task
            };
        }
    }
}
