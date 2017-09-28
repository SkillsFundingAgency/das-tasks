using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Tasks.Application.Queries.GetTasksByOwnerId;
using SFA.DAS.Tasks.API.Controllers;
using SFA.DAS.Tasks.API.Types.DTOs;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Domain.Models;

namespace SFA.DAS.Tasks.API.UnitTests.Controllers.TaskControllerTests
{
    public class WhenIGetTasks
    {
        private const string OwnerId = "1234";
        private const string DifferentOwnerId = "differentOwner";

        private TaskController _controller;
        private Mock<IMediator> _mediator;
        private List<DasTask> _tasks;

        [SetUp]
        public void Arrange()
        {
            _tasks = new List<DasTask>
            {
                new DasTask
                {
                    OwnerId = "123",
                    Type = TaskType.AgreementToSign,
                    ItemsDueCount = 2
                }
            };

            _mediator = new Mock<IMediator>();

            _mediator.Setup(x => x.SendAsync(It.Is<GetTasksByOwnerIdRequest>(a => a.OwnerId == OwnerId)))
                .ReturnsAsync(new GetTasksByOwnerIdResponse { Tasks = _tasks });

            _controller = new TaskController(_mediator.Object);
        }

        [Test]
        public async Task GivenThereAreTasksThenIShouldGetOkResultWithTasks()
        {
            //Act
            var response = await _controller.GetTasks(OwnerId);
            _mediator.Verify(x => x.SendAsync(It.Is<GetTasksByOwnerIdRequest>(request => request.OwnerId.Equals(OwnerId))), Times.Once);

            var result = response as OkNegotiatedContentResult<IEnumerable<TaskDto>>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.Count());

            var taskDto = result.Content.First();

            Assert.AreEqual(_tasks[0].OwnerId, taskDto.OwnerId);
            Assert.AreEqual(_tasks[0].Type.ToString(), taskDto.Type);
            Assert.AreEqual(_tasks[0].ItemsDueCount, taskDto.ItemsDueCount);
        }

        [Test]
        public async Task GivenThereAreTasksButIamADifferentOwnerThenIShouldGetOkResultWithNullTasks()
        {
            //Act
            var response = await _controller.GetTasks(DifferentOwnerId);
            _mediator.Verify(x => x.SendAsync(It.Is<GetTasksByOwnerIdRequest>(request => request.OwnerId.Equals(DifferentOwnerId))), Times.Once);

            var result = response as OkNegotiatedContentResult<IEnumerable<TaskDto>>;

            //Assert
            Assert.IsNotNull(result);
            Assert.False(result.Content.Any());
        }

        [Test]
        public async Task GivenThereAreNoTasksThenIShouldGetOkResultWithZeroTasks()
        {
            //Arrange
            _mediator.Setup(x => x.SendAsync(It.Is<GetTasksByOwnerIdRequest>(a => a.OwnerId == OwnerId)))
                .ReturnsAsync(new GetTasksByOwnerIdResponse { Tasks = null });

            //Act
            var response = await _controller.GetTasks(OwnerId);
            _mediator.Verify(x => x.SendAsync(It.Is<GetTasksByOwnerIdRequest>(request => request.OwnerId.Equals(OwnerId))), Times.Once);

            var result = response as OkNegotiatedContentResult<IEnumerable<TaskDto>>;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.Content);
        }
    }
}
