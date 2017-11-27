using NUnit.Framework;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Application.UnitTests.Commands.SaveTaskCommandTests
{
    public class WhenIValidateTheRequest
    {
        private SaveTaskCommandValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new SaveTaskCommandValidator();
        }

        [Test]
        public void ThenIShouldPassValidationWithAValidCompletedTaskRequest()
        {
            //Arrange
            var request = new SaveTaskCommand { EmployerAccountId = "1234", Type = TaskType.AddApprentices, TaskCompleted = true};

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.IsTrue(result.IsValid());
        }

        [Test]
        public void ThenIShouldPassValidationWithAValidRequest()
        {
            //Arrange
            var request = new SaveTaskCommand { EmployerAccountId = "1234", Type = TaskType.AddApprentices };

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.IsTrue(result.IsValid());
        }

        [Test]
        public void ThenIShouldFailValidationIfEmployerAccountIdIsNotPresent()
        {
            //Arrange
            
            var request = new SaveTaskCommand { Type = TaskType.AddApprentices, TaskCompleted = true };

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.IsFalse(result.IsValid());
            Assert.AreEqual("Cannot save task when employer account ID is not given.", result.ValidationDictionary[nameof(request.EmployerAccountId)]);
        }

        [Test]
        public void ThenIShouldFailValidationIfTaskTypeIsNotPresent()
        {
            //Arrange
            var request = new SaveTaskCommand { EmployerAccountId = "1234", TaskCompleted = true };

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.IsFalse(result.IsValid());
            Assert.AreEqual("Cannot save task when task type is not given.", result.ValidationDictionary[nameof(request.Type)]);
        }
    }
}
