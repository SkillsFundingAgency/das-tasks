using MediatR;
using SFA.DAS.Tasks.Domain.Models;

namespace SFA.DAS.Tasks.Application.Commands.SaveTask
{
    public class SaveTaskCommand : IAsyncRequest<SaveTaskCommandResponse>
    {
        public DasTask Task { get; set; }   
    }
}
