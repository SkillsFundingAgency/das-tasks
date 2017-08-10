using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using MediatR;
using SFA.DAS.Tasks.Application.Queries.GetTasksByOwnerId;
using SFA.DAS.Tasks.API.Types.DTOs;

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

            return Ok(result.Todos);
        }
      
    }
}
