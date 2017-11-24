using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Tasks.Application.Queries.GetTasksByOwnerId;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Domain.Models;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.Application.UnitTests.Queries.GetTasksByOwnerIdTests
{
    public class WhenIGetTasks : QueryBaseTest<GetTasksByOwnerIdHandler, GetTasksByOwnerIdRequest, GetTasksByOwnerIdResponse>
    {
        private const string TaskOwnerId = "123ACX";

        private Mock<ITaskRepository> _repository;
        private List<DasTask> _tasks;
        private List<DasTask> _monthlyRemindertasks;

        public override GetTasksByOwnerIdRequest Query { get; set; }
        public override GetTasksByOwnerIdHandler RequestHandler { get; set; }
        public override Mock<IValidator<GetTasksByOwnerIdRequest>> RequestValidator { get; set; }

        [SetUp]
        public void Arrange()
        {
            base.SetUp();

            _tasks = new List<DasTask>
            {
                new DasTask
                {
                    Type = TaskType.AgreementToSign
                }
            };

            _monthlyRemindertasks = new List<DasTask>
            {
                new DasTask
                {
                    Type = TaskType.LevyDeclarationDue
                }
            };

            _repository = new Mock<ITaskRepository>();
            _repository.Setup(x => x.GetTasks(It.IsAny<string>())).ReturnsAsync(_tasks);
            _repository.Setup(x => x.GetMonthlyReminderTasks(It.IsAny<string>())).ReturnsAsync(_monthlyRemindertasks);
            _repository.Setup(x => x.GetUserTaskSuppressions(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<TaskType>());

            RequestHandler = new GetTasksByOwnerIdHandler(_repository.Object, RequestValidator.Object);
            Query = new GetTasksByOwnerIdRequest{ OwnerId = TaskOwnerId, UserId = "DEF123"};
        }
       
        [Test]
        public override async Task ThenIfTheMessageIsValidTheTasksAreReturned()
        {
            //Arrange
            var expectedTasks = _tasks.Concat(_monthlyRemindertasks).ToList();

            //Act
            var result = await RequestHandler.Handle(Query);

            //Assert
            _repository.Verify(x => x.GetTasks(TaskOwnerId), Times.Once);
            _repository.Verify(x => x.GetMonthlyReminderTasks(TaskOwnerId), Times.Once);
            Assert.AreEqual(expectedTasks, result.Tasks);
        }

        [Test]
        public async Task ThenIShouldNotSeeTasksRemindersThatIHaveDimissed()
        {
            //Arrange
            var dismissedTaskType = _monthlyRemindertasks.First().Type;

            _repository.Setup(x => x.GetUserTaskSuppressions(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new List<TaskType>
            {
                dismissedTaskType
            });
           

            //Act
            var result = await RequestHandler.Handle(Query);

            //Assert
            _repository.Verify(x => x.GetUserTaskSuppressions(Query.UserId, Query.OwnerId), Times.Once);
            Assert.IsFalse(result.Tasks.Any(t => t.Type == dismissedTaskType));
        }

        [Test] public async Task ThenIShouldBeAbleToGetTasksUsingOlderClients()
        {
            //Arrange
            Query.UserId = null;

            //Act
            await RequestHandler.Handle(Query);

            //Assert
            _repository.Verify(x => x.GetUserTaskSuppressions(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
