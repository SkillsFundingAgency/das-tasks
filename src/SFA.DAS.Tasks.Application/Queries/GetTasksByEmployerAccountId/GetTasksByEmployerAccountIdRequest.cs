using MediatR;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Application.Queries.GetTasksByEmployerAccountId
{
    public class GetTasksByEmployerAccountIdRequest : IRequest<GetTasksByEmployerAccountIdResponse>
    {
        public string EmployerAccountId { get; set; }
        public string UserId { get; set; }
        public ApprenticeshipEmployerType ApplicableToApprenticeshipEmployerType { get; set; }
    }
}
