using System;
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
            var response =await _mediator.SendAsync<GetTasksByOwnerIdResponse>(new GetTasksByOwnerIdRequest
            {
                OwnerId = ownerId
            });
            return response;
        }
      
    }
}
