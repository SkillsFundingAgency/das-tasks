using System;
using System.Web.Http;


namespace SFA.DAS.Tasks.API.Controllers
{
    [RoutePrefix("api/tasks/{ownerId}")]
    public class TasksController : ApiController
    {
        [Route("")]
        public IHttpActionResult GetTasks(string ownerId)
        {
            throw new NotImplementedException();
        }

      
    }
}
