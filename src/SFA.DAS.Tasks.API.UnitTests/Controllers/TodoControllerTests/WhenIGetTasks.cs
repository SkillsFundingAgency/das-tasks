using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private const string DifferentOwnerId = "differentOwner";

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

            _controller = new TodoController(_mediator.Object);
        }

        private void SetupToReturnTodos()
        {
            _mediator.Setup(x => x.SendAsync(It.Is<GetTasksByOwnerIdRequest>(a => a.OwnerId == OwnerId)))
                .ReturnsAsync(new GetTasksByOwnerIdResponse { Todos = _todos });
        }

        private void SetupToReturnNoTodos()
        {
            _mediator.Setup(x => x.SendAsync(It.Is<GetTasksByOwnerIdRequest>(a => a.OwnerId == OwnerId)))
                .ReturnsAsync(new GetTasksByOwnerIdResponse { Todos = null });
        }

        //[Test]
        //public async Task ThenIfThereAreTodosTasksShouldBeRequested()
        //{
        //    //Arrange
        //    SetupToReturnTodos();

        //    //Act
        //    await _controller.GetTasks(OwnerId);

        //    //Assert
        //    _mediator.Verify(x => x.SendAsync(It.Is<GetTasksByOwnerIdRequest>(request =>request.OwnerId.Equals(OwnerId))), Times.Once);
        //}

        [Test]
        public async Task GivenThereAreTodosThenIShouldGetOkResultWithTodos()
        {
            //Arrange
            SetupToReturnTodos();

            //Act
            var response = await _controller.GetTasks(OwnerId);
            _mediator.Verify(x => x.SendAsync(It.Is<GetTasksByOwnerIdRequest>(request => request.OwnerId.Equals(OwnerId))), Times.Once);

            var result = response as OkNegotiatedContentResult<IEnumerable<Todo>>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_todos, result.Content);
        }

        [Test]
        public async Task GivenThereAreTodosButIamADifferentOwnerThenIShouldGetOkResultWithNullTodos()
        {
            //Arrange
            SetupToReturnTodos();

            //Act
            var response = await _controller.GetTasks(DifferentOwnerId);
            _mediator.Verify(x => x.SendAsync(It.Is<GetTasksByOwnerIdRequest>(request => request.OwnerId.Equals(DifferentOwnerId))), Times.Once);

            var result = response as OkNegotiatedContentResult<IEnumerable<Todo>>;

            //Assert
            Assert.IsNotNull(result);
            Assert.False(result.Content.Any());
        }

        //[Test]
        //public async Task ThenIfThereAreNoTodosTasksShouldBeRequested()
        //{
        //    //Arrange
        //    SetupToReturnTodos();

        //    //Act
        //    await _controller.GetTasks(OwnerId);

        //    //Assert
        //    _mediator.Verify(x => x.SendAsync(It.Is<GetTasksByOwnerIdRequest>(request => request.OwnerId.Equals(OwnerId))), Times.Once);
        //}

        [Test]
        public async Task GivenThereAreNoTodosThenIShouldGetOkResultWithNullTodos()
        {
            //Arrange
            SetupToReturnNoTodos();

            //Act
            var response = await _controller.GetTasks(OwnerId);
            _mediator.Verify(x => x.SendAsync(It.Is<GetTasksByOwnerIdRequest>(request => request.OwnerId.Equals(OwnerId))), Times.Once);

            var result = response as OkNegotiatedContentResult<IEnumerable<Todo>>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.Content);
        }


    }
}
