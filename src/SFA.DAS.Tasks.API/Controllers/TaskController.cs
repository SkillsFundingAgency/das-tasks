using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.Tasks.Application.Queries.GetTasksByOwnerId;
using SFA.DAS.Tasks.API.Attributes;
using SFA.DAS.Tasks.API.Types.DTOs;

namespace SFA.DAS.Tasks.API.Controllers
{
    [RoutePrefix("api/tasks/{ownerId}")]
    public class TaskController : ApiController
    {
        private readonly IMediator _mediator;

        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("")]
        [ApiAuthorize(Roles = "ReadOwnerTasks")]
        public async Task<IHttpActionResult> GetTasks(string ownerId)
        {
            var result = await _mediator.SendAsync(new GetTasksByOwnerIdRequest
            {
                OwnerId = ownerId
            });

            if (result?.Tasks == null) return Ok(Enumerable.Empty<TaskDto>());

            var tasks = result.Tasks.Select(x => new TaskDto
            {
                OwnerId = x.OwnerId,
                Type = x.Type.ToString(),
                ItemsDueCount = x.ItemsDueCount
            }).AsEnumerable();

            return Ok(tasks);
        }
    }
}
