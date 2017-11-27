using MediatR;

namespace SFA.DAS.Tasks.Application.Queries.GetTasksByEmployerAccountId
{
    public class GetTasksByEmployerAccountIdRequest : IAsyncRequest<GetTasksByEmployerAccountIdResponse>
    {
        public string EmployerAccountId { get; set; }
        public string UserId { get; set; }
    }
}
