using System.Collections.Generic;
using System.Web.Http.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Tasks.Application.Queries.GetTasksByOwnerId;
using SFA.DAS.Tasks.API.Controllers;
using SFA.DAS.Tasks.Domain.Models;

namespace SFA.DAS.Tasks.API.UnitTests.Controllers.TodoControllerTests
{
    public class WhenIGetTasks
    {
        private const string OwnerId = "1234";

        private TodoController _controller;
        private Mock<IMediator> _mediator;
        private List<Todo> _todos;

        [SetUp]
        public void Arrange()
        {
            _todos = new List<Todo>
            {
                new Todo()
            };

            _mediator = new Mock<IMediator>();

            _mediator.Setup(x => x.SendAsync(It.Is<GetTasksByOwnerIdRequest>(a=>a.OwnerId==OwnerId)))
                     .ReturnsAsync(new GetTasksByOwnerIdResponse{ Todos = _todos });

            _controller = new TodoController(_mediator.Object);
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
            var result = response as OkNegotiatedContentResult<IEnumerable<Todo>>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_todos, result.Content);

        }
    }
}
