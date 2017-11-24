﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveMonthlyUserDismiss;
using SFA.DAS.Tasks.Application.Queries.GetTasksByOwnerId;
using SFA.DAS.Tasks.API.Attributes;
using SFA.DAS.Tasks.API.Types.DTOs;

namespace SFA.DAS.Tasks.API.Controllers
{
    [RoutePrefix("api/tasks/{ownerId}")]
    public class TaskController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILog _logger;

        public TaskController(IMediator mediator, ILog logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Route("", Name = "GetTasks")]
        [ApiAuthorize(Roles = "ReadOwnerTasks")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTasks(string ownerId)
        {
            _logger.Debug($"Getting tasks for owner {ownerId}");

            var result = await _mediator.SendAsync(new GetTasksByOwnerIdRequest
            {
                OwnerId = ownerId
            });

            if (result?.Tasks == null)
                return Ok(Enumerable.Empty<TaskDto>());

            var tasks = result.Tasks.Select(x => new TaskDto
            {
                OwnerId = x.OwnerId,
                Type = x.Type.ToString(),
                ItemsDueCount = x.ItemsDueCount
            }).AsEnumerable();

            return Ok(tasks);
        }

        [Route("supressions/{UserId}/add/{taskType}", Name = "AddSupression")]
        [ApiAuthorize(Roles = "AddUserReminderSupressions")]
        [HttpPost]
        public async Task<IHttpActionResult> AddUserReminderSupression(string ownerId, string userId, string taskType)
        {
            _logger.Debug($"Adding task reminder supression for ownerId {ownerId}, user Id {userId} and task type {taskType}");

            await _mediator.SendAsync(new SaveUserReminderSupressionFlagCommand
            {
                AccountId = ownerId,
                UserId = userId,
                TaskType = taskType
            });

            _logger.Debug($"Task reminder supression added for ownerId {ownerId}, user Id {userId} and task type {taskType}");

            return Ok();
        }
    }
}
