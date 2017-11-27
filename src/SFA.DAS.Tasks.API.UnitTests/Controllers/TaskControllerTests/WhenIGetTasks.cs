using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Queries.GetTasksByEmployerAccountId;
using SFA.DAS.Tasks.API.Controllers;
using SFA.DAS.Tasks.API.Types.DTOs;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Domain.Models;

namespace SFA.DAS.Tasks.API.UnitTests.Controllers.TaskControllerTests
{
    public class WhenIGetTasks
    {
        private const string EmployerAccountId = "1234";
        private const string DifferentEmployerAccountId = "differentAccount";

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
                    EmployerAccountId = "123",
                    Type = TaskType.AgreementToSign,
                    ItemsDueCount = 2
                }
            };

            _mediator = new Mock<IMediator>();

            _mediator.Setup(x => x.SendAsync(It.Is<GetTasksByEmployerAccountIdRequest>(a => a.EmployerAccountId == EmployerAccountId)))
                .ReturnsAsync(new GetTasksByEmployerAccountIdResponse { Tasks = _tasks });

            _controller = new TaskController(_mediator.Object, Mock.Of<ILog>());
        }

        [Test]
        public async Task GivenThereAreTasksThenIShouldGetOkResultWithTasks()
        {
            //Act
            var response = await _controller.GetTasks(EmployerAccountId);
            var result = response as OkNegotiatedContentResult<IEnumerable<TaskDto>>;

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<GetTasksByEmployerAccountIdRequest>(request => request.EmployerAccountId.Equals(EmployerAccountId))), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.Count());

            var taskDto = result.Content.First();

            Assert.AreEqual(_tasks[0].EmployerAccountId, taskDto.EmployerAccountId);
            Assert.AreEqual(_tasks[0].Type.ToString(), taskDto.Type);
            Assert.AreEqual(_tasks[0].ItemsDueCount, taskDto.ItemsDueCount);
        }

        [Test]
        public async Task GivenThereAreTasksButIamADifferentOwnerThenIShouldGetOkResultWithNullTasks()
        {
            //Act
            var response = await _controller.GetTasks(DifferentEmployerAccountId);
            var result = response as OkNegotiatedContentResult<IEnumerable<TaskDto>>;

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<GetTasksByEmployerAccountIdRequest>(request => request.EmployerAccountId.Equals(DifferentEmployerAccountId))), Times.Once);
            Assert.IsNotNull(result);
            Assert.False(result.Content.Any());
        }

        [Test]
        public async Task GivenThereAreNoTasksThenIShouldGetOkResultWithZeroTasks()
        {
            //Arrange
            _mediator.Setup(x => x.SendAsync(It.Is<GetTasksByEmployerAccountIdRequest>(a => a.EmployerAccountId == EmployerAccountId)))
                .ReturnsAsync(new GetTasksByEmployerAccountIdResponse { Tasks = null });

            //Act
            var response = await _controller.GetTasks(EmployerAccountId);
            var result = response as OkNegotiatedContentResult<IEnumerable<TaskDto>>;

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<GetTasksByEmployerAccountIdRequest>(request => request.EmployerAccountId.Equals(EmployerAccountId))), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.Content);
        }
    }
}
