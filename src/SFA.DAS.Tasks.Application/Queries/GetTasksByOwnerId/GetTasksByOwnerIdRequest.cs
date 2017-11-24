using MediatR;

namespace SFA.DAS.Tasks.Application.Queries.GetTasksByOwnerId
{
    public class GetTasksByOwnerIdRequest : IAsyncRequest<GetTasksByOwnerIdResponse>
    {
        public string OwnerId { get; set; }
        public string UserId { get; set; }
    }
}
