using System;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.Tasks.Application.Queries.GetTask
{
    public class GetTaskRequestHandler : IAsyncRequestHandler<GetTaskRequest, GetTaskResponse>
    {
        public Task<GetTaskResponse> Handle(GetTaskRequest message)
        {
            throw new NotImplementedException();
        }
    }
}
