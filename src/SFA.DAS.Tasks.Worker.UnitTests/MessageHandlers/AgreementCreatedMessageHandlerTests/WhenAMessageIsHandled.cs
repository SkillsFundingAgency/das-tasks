using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.Application.Queries.GetTask;
using SFA.DAS.Tasks.Domain.Enums;
using SFA.DAS.Tasks.Domain.Models;
using SFA.DAS.Tasks.Worker.MessageHandlers;

namespace SFA.DAS.Tasks.Worker.UnitTests.MessageHandlers.AgreementCreatedMessageHandlerTests
{
    public class WhenAMessageIsHandled
    {
        private Mock<IMediator> _mediator;
        private AgreementCreatedMessageHandler _handler;
        private AgreementCreatedMessage _message;
        private DasTask _task;

        [SetUp]
        public void Arrange()
        {
            _mediator = new Mock<IMediator>();

            _handler = new AgreementCreatedMessageHandler(_mediator.Object);

            _message = new AgreementCreatedMessage
            {
                AccountId = 2,
                LegalEntityId = 4
            };

            _task = new DasTask
            {
                TaskOwnerId = _message.AccountId.ToString(),
                Type = TaskType.AgreementToSign,
                ItemsDueCount = 1
            };
        }

        [Test]
        public void ThenANewTaskShouldBeCreated()
        {
            //Arrange
            _mediator.Setup(x => x.SendAsync(It.IsAny<GetTaskRequest>()))
                .ReturnsAsync(new GetTaskResponse());

            //Act
            _handler.Handle(_message);

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<SaveTaskCommand>(c => 
                c.Task != null &&
                c.Task.TaskOwnerId.Equals(_message.AccountId.ToString()) &&
                c.Task.Type.Equals(TaskType.AgreementToSign) &&
                c.Task.ItemsDueCount.Equals(1))), Times.Once());
        }

        [Test]
        public void ThenAnExistingTaskShouldBeUpdated()
        {
            //Arrange
            var expectedItemCount = _task.ItemsDueCount + 1;
            _mediator.Setup(x => x.SendAsync(It.IsAny<GetTaskRequest>()))
                .ReturnsAsync(new GetTaskResponse { Task = _task });

            //Act
            _handler.Handle(_message);

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<SaveTaskCommand>(c =>
                c.Task != null &&
                c.Task.TaskOwnerId.Equals(_message.AccountId.ToString()) &&
                c.Task.Type.Equals(TaskType.AgreementToSign) &&
                c.Task.ItemsDueCount.Equals(expectedItemCount))), Times.Once());
        }
    }
}
