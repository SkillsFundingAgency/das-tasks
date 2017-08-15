using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.Application.Commands.SaveTask
{
    public class SaveTaskCommandHandler : IAsyncRequestHandler<SaveTaskCommand, SaveTaskCommandResponse>
    {
        public SaveTaskCommandHandler(ITaskRepository repositoryObject)
        {
            
        }

        public Task<SaveTaskCommandResponse> Handle(SaveTaskCommand message)
        {
            throw new System.NotImplementedException();
        }
    }
}
