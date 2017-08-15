using NUnit.Framework;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.Domain.Enums;
using SFA.DAS.Tasks.Domain.Models;

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
        public void ThenIShouldPassValidationWithAValidRequest()
        {
            //Arrange
            var task = new DasTask {OwnerId = "1234", Type = TaskType.AddApprentices, ItemsDueCount = 1};
            var request = new SaveTaskCommand { Task = task};

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.IsTrue(result.IsValid());
        }

        [Test]
        public void ThenIShouldFailValidationIfOwnerIdIsNotPresent()
        {
            //Arrange
            var task = new DasTask { Type = TaskType.AddApprentices, ItemsDueCount = 1 };
            var request = new SaveTaskCommand { Task = task };

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.IsFalse(result.IsValid());
            Assert.AreEqual("Cannot save task when owner ID is not given.", result.ValidationDictionary[nameof(request.Task.OwnerId)]);
        }

        [Test]
        public void ThenIShouldFailValidationIfTaskTypeIsNotPresent()
        {
            //Arrange
            var task = new DasTask { OwnerId = "1234", ItemsDueCount = 1 };
            var request = new SaveTaskCommand { Task = task };

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.IsFalse(result.IsValid());
            Assert.AreEqual("Cannot save task when task type is not given.", result.ValidationDictionary[nameof(request.Task.Type)]);
        }

        [Test]
        public void ThenIShouldFailValidationIfItemsDueIsZero()
        {
            //Arrange
            var task = new DasTask { OwnerId = "1234", Type = TaskType.AddApprentices, ItemsDueCount = 0 };
            var request = new SaveTaskCommand { Task = task };

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.IsFalse(result.IsValid());
            Assert.AreEqual("Cannot save task when task type is not given.", result.ValidationDictionary[nameof(request.Task.ItemsDueCount)]);
        }
    }
}
