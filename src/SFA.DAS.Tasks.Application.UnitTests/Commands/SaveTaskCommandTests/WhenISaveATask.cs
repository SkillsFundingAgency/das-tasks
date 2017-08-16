using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.Domain.Enums;
using SFA.DAS.Tasks.Domain.Models;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.Application.UnitTests.Commands.SaveTaskCommandTests
{
    public class WhenISaveATask : QueryBaseTest<SaveTaskCommandHandler, SaveTaskCommand, SaveTaskCommandResponse>
    {
        private Mock<ITaskRepository> _repository;
        private DasTask _task;

        public override SaveTaskCommand Query { get; set; }
        public override SaveTaskCommandHandler RequestHandler { get; set; }
        public override Mock<IValidator<SaveTaskCommand>> RequestValidator { get; set; }

        [SetUp]
        public void Arrange()
        {
            base.SetUp();

            _task = new DasTask
            {
                OwnerId = "123",
                ItemsDueCount = 1,
                Type = TaskType.AddApprentices
            };

            _repository = new Mock<ITaskRepository>();

            RequestHandler = new SaveTaskCommandHandler(_repository.Object, RequestValidator.Object);
            Query = new SaveTaskCommand {Task = _task};
        }
      
        [Test]
        public override async Task ThenIfTheMessageIsValidTheRepositoryIsCalled()
        {
            //Act
            await RequestHandler.Handle(Query);

            //Assert
            _repository.Verify(x => x.SaveTask(_task), Times.Once);
        }

        public override async Task ThenIfTheMessageIsValidTheValueIsReturnedInTheResponse()
        {
            //Act
            var response = await RequestHandler.Handle(Query);

            //Assert
            Assert.IsNotNull(response);
        }
    }
}
