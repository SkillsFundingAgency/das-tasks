using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Tasks.Application.Exceptions;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.Domain.Models;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.Application.Commands.SaveTask
{
    public class SaveTaskCommandHandler : IAsyncRequestHandler<SaveTaskCommand, SaveTaskCommandResponse>
    {
        private readonly ITaskRepository _repository;
        private readonly IValidator<SaveTaskCommand> _validator;

        public SaveTaskCommandHandler(ITaskRepository repository, IValidator<SaveTaskCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<SaveTaskCommandResponse> Handle(SaveTaskCommand message)
        {
            var validationResult = _validator.Validate(message);

            if (!validationResult.IsValid())
            {
                throw new InvalidRequestException(validationResult.ValidationDictionary);
            }

            var task = await _repository.GetTask(message.OwnerId, message.Type) ?? new DasTask
            {
                OwnerId = message.OwnerId,
                Type = message.Type
            };

            //No current task present so we don't need to decrement or create a task
            if (task.ItemsDueCount == 0 && message.TaskCompleted)
            {
                return new SaveTaskCommandResponse();
            }

            task.ItemsDueCount += (ushort)(message.TaskCompleted ? -1 : 1);

            await _repository.SaveTask(task);

            return new SaveTaskCommandResponse();
        }
    }
}
