using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.Tasks.Application.Queries.GetTasksByOwnerId;


namespace SFA.DAS.Tasks.API.Controllers
{
    [RoutePrefix("api/tasks/{ownerId}")]
    public class TasksController : ApiController
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
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

            return Ok(result);
        }
      
    }
}
