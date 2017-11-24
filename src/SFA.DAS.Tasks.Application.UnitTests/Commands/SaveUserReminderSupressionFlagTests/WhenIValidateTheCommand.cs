using NUnit.Framework;
using SFA.DAS.Tasks.Application.Commands.SaveMonthlyUserDismiss;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Application.UnitTests.Commands.SaveUserReminderSupressionFlagTests
{
    public class WhenIValidateTheCommand
    {
        private SaveUserReminderSupressionFlagCommandValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new SaveUserReminderSupressionFlagCommandValidator();
        }

        [Test]
        public void ThenItShouldPassValidation()
        {
            //Arrange
            var command = new SaveUserReminderSupressionFlagCommand
            {
                AccountId = "ABC123",
                UserId = "DEF123",
                TaskType = TaskType.AgreementToSign.ToString()
            };

            //Act
            var result = _validator.Validate(command);

            //Assert
            Assert.IsTrue(result.IsValid());
        }

        [Test]
        public void ThenItShouldFailValidationIfTheCommandIsInvalid()
        {
            //Arrange
            var command = new SaveUserReminderSupressionFlagCommand
            {
                AccountId = string.Empty,
                UserId = string.Empty,
                TaskType = "Not Supported"
            };

            //Act
            var result = _validator.Validate(command);

            //Assert
            Assert.IsFalse(result.IsValid());
            Assert.IsTrue(result.ValidationDictionary.ContainsKey(nameof(command.AccountId)));
            Assert.IsTrue(result.ValidationDictionary.ContainsKey(nameof(command.UserId)));
            Assert.IsTrue(result.ValidationDictionary.ContainsKey(nameof(command.TaskType)));
        }
    }
}
