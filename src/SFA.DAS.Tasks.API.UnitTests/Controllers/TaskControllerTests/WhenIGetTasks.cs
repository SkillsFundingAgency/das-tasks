using System.Collections.Generic;
using System.Web.Http.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Tasks.Application.Queries.GetTasksByOwnerId;
using SFA.DAS.Tasks.API.Controllers;
using SFA.DAS.Tasks.API.Types.DTOs;
using SFA.DAS.Tasks.Domain.Models;

namespace SFA.DAS.Tasks.API.UnitTests.Controllers.TaskControllerTests
{
    public class WhenIGetTasks
    {
        private const string OwnerId = "1234";

        private TasksController _controller;
        private Mock<IMediator> _mediator;
        private List<Task> _tasks;

        [SetUp]
        public void Arrange()
        {
            _tasks = new List<Task>
            {
                new Task()
            };

            _mediator = new Mock<IMediator>();

            _mediator.Setup(x => x.SendAsync(It.IsAny<GetTasksByOwnerIdRequest>()))
                     .ReturnsAsync(new GetTasksByOwnerIdResponse{ Tasks = _tasks });

            _controller = new TasksController(_mediator.Object);
        }

        [Test]
        public async System.Threading.Tasks.Task ThenTasksShouldBeRequested()
        {
            //Act
            await _controller.GetTasks(OwnerId);

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<GetTasksByOwnerIdRequest>(request =>request.OwnerId.Equals(OwnerId))), Times.Once);
        }

        [Test]
        public async System.Threading.Tasks.Task ThenIShouldGetTheAvailableTasks()
        {
            //Act
            var response = await _controller.GetTasks(OwnerId);
            var result = response as OkNegotiatedContentResult<IEnumerable<Task>>;//taskDto?

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_tasks, result.Content);

        }
    }
}
