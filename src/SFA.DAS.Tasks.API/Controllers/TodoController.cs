using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.Tasks.Application.Queries.GetTasksByOwnerId;
using SFA.DAS.Tasks.Domain.Models;
using SFA.DAS.Tasks.API.Types.DTOs

namespace SFA.DAS.Tasks.API.Controllers
{
    [RoutePrefix("api/tasks/{ownerId}")]
    public class TodoController : ApiController
    {
        private readonly IMediator _mediator;

        public TodoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("")]
        public async Task<IHttpActionResult> GetTasks(string ownerId)
        {
            var result = await _mediator.SendAsync(new GetTasksByOwnerIdRequest
            {
                OwnerId = ownerId
            });

            if (result == null) return Ok(Enumerable.Empty<Todo>());

            return Ok(result.Todos);
        }
      
    }
}
