﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveUserReminderSuppression;
using SFA.DAS.Tasks.Application.Queries.GetTasksByEmployerAccountId;
using SFA.DAS.Tasks.API.Attributes;
using SFA.DAS.Tasks.API.Types.DTOs;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.API.Controllers
{
    [RoutePrefix("api/tasks/{employerAccountId}")]
    public class TaskController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILog _logger;

        public TaskController(IMediator mediator, ILog logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Obsolete("This method is being phased out and replaced by GetUserTasks")]
        [Route("", Name = "GetTasks")]
        [ApiAuthorize(Roles = "ReadOwnerTasks")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTasks(string employerAccountId)
        {
            //This method is here to support clients that are older than the current breaking change
            return await GetUserTasks(employerAccountId, string.Empty, ApprenticeshipEmployerType.All);
        }

        [Route("{userId}", Name = "GetUserTasks")]
        [ApiAuthorize(Roles = "ReadOwnerTasks")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserTasks(string employerAccountId, string userId, ApprenticeshipEmployerType applicableToApprenticeshipEmployerType)
        {
            _logger.Debug($"Getting tasks for employer account {employerAccountId}");

            var result = await _mediator.SendAsync(new GetTasksByEmployerAccountIdRequest
            {
                EmployerAccountId = employerAccountId,
                UserId = userId,
                ApplicableToApprenticeshipEmployerType = applicableToApprenticeshipEmployerType
            });

            if (result?.Tasks == null)
                return Ok(Enumerable.Empty<TaskDto>());

            var tasks = result.Tasks.Select(x => new TaskDto
            {
                EmployerAccountId = x.EmployerAccountId,
                Type = x.Type.ToString(),
                ItemsDueCount = x.ItemsDueCount
            }).AsEnumerable();

            return Ok(tasks);
        }

        [Route("supressions/{userId}/add/{taskType}", Name = "AddSupression")]
        [ApiAuthorize(Roles = "AddUserReminderSupressions")]
        [HttpPost]
        public async Task<IHttpActionResult> AddUserReminderSupression(string employerAccountId, string userId, string taskType)
        {
            _logger.Debug($"Adding task reminder supression for employer account Id {employerAccountId}, user Id {userId} and task type {taskType}");

            await _mediator.SendAsync(new SaveUserReminderSuppressionFlagCommand
            {
                EmployerAccountId = employerAccountId,
                UserId = userId,
                TaskType = taskType
            });

            _logger.Debug($"Task reminder supression added for employer account Id {employerAccountId}, user Id {userId} and task type {taskType}");

            return Ok();
        }
    }
}
