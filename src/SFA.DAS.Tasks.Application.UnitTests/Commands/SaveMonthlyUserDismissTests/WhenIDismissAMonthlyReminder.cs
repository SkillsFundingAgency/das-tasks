using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveMonthlyUserDismiss;
using SFA.DAS.Tasks.Application.Exceptions;
using SFA.DAS.Tasks.Application.Validation;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.Application.UnitTests.Commands.SaveMonthlyUserDismissTests
{
    public class WhenIDismissAMonthlyReminder
    {
        private SaveMonthlyReminderDismissCommandHandler _handler;
        private Mock<ITaskRepository> _repository;
        private SaveMonthlyReminderDismissCommand _command;
        private TaskType _taskType;
        private Mock<ILog> _logger;
        private Mock<IValidator<SaveMonthlyReminderDismissCommand>> _validator;

        [SetUp]
        public void Arrange()
        {
            _taskType = TaskType.LevyDeclarationDue;

            _repository = new Mock<ITaskRepository>();
            _logger = new Mock<ILog>();
            _validator = new Mock<IValidator<SaveMonthlyReminderDismissCommand>>();

            _validator.Setup(x => x.Validate(It.IsAny<SaveMonthlyReminderDismissCommand>()))
                .Returns(new ValidationResult());

            _handler = new SaveMonthlyReminderDismissCommandHandler(_repository.Object, _logger.Object, _validator.Object);
            _command = new SaveMonthlyReminderDismissCommand
            {
                AccountId = 10,
                UserId = 15,
                TaskType = _taskType.ToString()
            };
        }

        [Test]
        public async Task ThenIShouldHaveMyDismissSaved()
        {
            //Act
            await _handler.Handle(_command);

            //Assert
            _validator.Verify(x => x.Validate(_command), Times.Once);
            _repository.Verify(x => x.SaveMonthlyReminderDismiss(_command.UserId, _command.AccountId, _taskType), Times.Once);
        }

        [Test]
        public void ThenIfMyRequestIsInvalidIShouldBeNotified()
        {
            //Arrange 
            _validator.Setup(x => x.Validate(It.IsAny<SaveMonthlyReminderDismissCommand>()))
                .Returns(new ValidationResult
                {
                    ValidationDictionary = new Dictionary<string, string>
                    {
                        {nameof(_command.TaskType),"Invalid value"}
                    }
                });

            //Act
            Assert.ThrowsAsync<InvalidRequestException>(async() => await _handler.Handle(_command));

            //Assert
            _validator.Verify(x => x.Validate(_command), Times.Once);
            _repository.Verify(x => x.SaveMonthlyReminderDismiss(_command.UserId, _command.AccountId, _taskType), Times.Never);
        }

        [Test]
        public void ThenIShouldBeInformedIfTheDismissedCouldNotBeSaved()
        {
            //Arrange
            _repository.Setup(x =>
                    x.SaveMonthlyReminderDismiss(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<TaskType>()))
                .Throws<Exception>();

            //Act
            Assert.ThrowsAsync<Exception>(async () =>
            {
                await _handler.Handle(_command);
            });
        }
    }
}
