using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Domain.Models;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.Application.UnitTests.Commands.SaveTaskCommandTests
{
    public class WhenISaveATask : QueryBaseTest<SaveTaskCommandHandler, SaveTaskCommand, SaveTaskCommandResponse>
    {
        private Mock<ITaskRepository> _repository;

        public override SaveTaskCommand Query { get; set; }
        public override SaveTaskCommandHandler RequestHandler { get; set; }
        public override Mock<IValidator<SaveTaskCommand>> RequestValidator { get; set; }

        [SetUp]
        public void Arrange()
        {
            base.SetUp();

            _repository = new Mock<ITaskRepository>();

            RequestHandler = new SaveTaskCommandHandler(_repository.Object, RequestValidator.Object);
            Query = new SaveTaskCommand
            {
                OwnerId = "123",
                TaskCompleted = false,
                Type = TaskType.AddApprentices
            };
        }
      
        [Test]
        public override async Task ThenIfTheMessageIsValidTheRepositoryIsCalled()
        {
            //Act
            await GetResponseFromRequestHandler();

            //Assert
            VerifyGetTaskCalledOnce();
            VerifySaveTaskCalledOnceWith(t => t.OwnerId.Equals(Query.OwnerId) &&
                                               t.Type.Equals(Query.Type) &&
                                               t.ItemsDueCount.Equals(1));
        }

        [Test]
        public override async Task ThenIfTheMessageIsValidTheValueIsReturnedInTheResponse()
        {
            //Act
            var response = await GetResponseFromRequestHandler();

            //Assert
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task ThenIfATaskIsAlreadySavedIShouldUpdateItsItemsDueCounter()
        {
            //We represent multiple tasks of the same type and owner as a counter rather than multiple tasks entries

            //Arrange
            var existingTask = CreateExistingDasTask();

            var expectedItemsDueCount = (ushort)(existingTask.ItemsDueCount + 1);

            SetupRepositoryToReturnExistingTask(existingTask);

            //Act
            await GetResponseFromRequestHandler();

            //Assert
            VerifyGetTaskCalledOnce();
            VerifySaveTaskCalledOnceWith(t => t.Id.Equals(existingTask.Id) &&
                                               t.OwnerId.Equals(existingTask.OwnerId) &&
                                               t.Type.Equals(existingTask.Type) &&
                                               t.ItemsDueCount.Equals(expectedItemsDueCount));
        }

        [Test]
        public async Task ThenIfATaskIsCompletedTheItemsDueCounterShouldBeDecremented()
        {
            //We represent multiple tasks of the same type and owner as a counter rather than multiple tasks entries

            //Arrange
            Query.TaskCompleted = true;

            var existingTask = CreateExistingDasTask();

            var expectedItemsDueCount = (ushort) (existingTask.ItemsDueCount - 1);

            SetupRepositoryToReturnExistingTask(existingTask);

            //Act
            await GetResponseFromRequestHandler();

            //Assert
            VerifyGetTaskCalledOnce();
            VerifySaveTaskCalledOnceWith(t => t.Id.Equals(existingTask.Id) &&
                                               t.OwnerId.Equals(existingTask.OwnerId) &&
                                               t.Type.Equals(existingTask.Type) &&
                                               t.ItemsDueCount.Equals(expectedItemsDueCount));
        }

        [Test]
        public async Task ThenDoNotSaveTheTaskIfItIsCompletedAndNoTasksAreCurrentlyStored()
        {
            //We represent multiple tasks of the same type and owner as a counter rather than multiple tasks entries

            //Arrange
            Query.TaskCompleted = true;

            //Act
            await GetResponseFromRequestHandler();

            //Assert
            VerifyGetTaskCalledOnce(); 
            _repository.Verify(x => x.SaveTask(It.IsAny<DasTask>()), Times.Never);
        }

        private static DasTask CreateExistingDasTask()
        {
            var existingTask = new DasTask
            {
                Id = Guid.NewGuid(),
                OwnerId = "123",
                Type = TaskType.AgreementToSign,
                ItemsDueCount = 3
            };
            return existingTask;
        }

        private void SetupRepositoryToReturnExistingTask(DasTask existingTask)
        {
            _repository.Setup(x => x.GetTask(Query.OwnerId, Query.Type)).ReturnsAsync(existingTask);
        }

        private async Task<SaveTaskCommandResponse> GetResponseFromRequestHandler()
        {
            var response = await RequestHandler.Handle(Query);
            return response;
        }

        private void VerifyGetTaskCalledOnce()
        {
            _repository.Verify(x => x.GetTask(Query.OwnerId, Query.Type), Times.Once);
        }

        private void VerifySaveTaskCalledOnceWith(Expression<Func<DasTask, bool>> taskExpression)
        {
            _repository.Verify(x => x.SaveTask(It.Is<DasTask>(taskExpression)), Times.Once);
        }
    }
}
