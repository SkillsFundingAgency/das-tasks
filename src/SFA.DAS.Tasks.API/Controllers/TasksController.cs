using System;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;


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
            throw new NotImplementedException();
        }
      
    }
}
