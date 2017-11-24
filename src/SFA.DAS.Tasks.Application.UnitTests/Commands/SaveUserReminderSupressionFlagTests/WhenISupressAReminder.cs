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
using SFA.DAS.Tasks.Domain.Models;
using SFA.DAS.Tasks.Domain.Repositories;

namespace SFA.DAS.Tasks.Application.UnitTests.Commands.SaveUserReminderSupressionFlagTests
{
    public class WhenISupressAReminder
    {
        private SaveUserReminderSupressionFlagCommandHandler _handler;
        private Mock<ITaskRepository> _repository;
        private SaveUserReminderSupressionFlagCommand _command;
        private TaskType _taskType;
        private Mock<ILog> _logger;
        private Mock<IValidator<SaveUserReminderSupressionFlagCommand>> _validator;

        [SetUp]
        public void Arrange()
        {
            _taskType = TaskType.LevyDeclarationDue;

            _repository = new Mock<ITaskRepository>();
            _logger = new Mock<ILog>();
            _validator = new Mock<IValidator<SaveUserReminderSupressionFlagCommand>>();

            _validator.Setup(x => x.Validate(It.IsAny<SaveUserReminderSupressionFlagCommand>()))
                .Returns(new ValidationResult());

            _handler = new SaveUserReminderSupressionFlagCommandHandler(_repository.Object, _logger.Object, _validator.Object);
            _command = new SaveUserReminderSupressionFlagCommand
            {
                AccountId = "ABC123",
                UserId = "DEF123",
                TaskType = _taskType.ToString()
            };
        }

        [Test]
        public async Task ThenThatSupressionShouldBeSaved()
        {
            //Act
            await _handler.Handle(_command);

            //Assert
            _validator.Verify(x => x.Validate(_command), Times.Once);
            _repository.Verify(x => x.SaveUserReminderSupression(It.Is<UserReminderSupressionFlag>
            (flag => flag.UserId == _command.UserId &&
                     flag.AccountId == _command.AccountId &&
                     flag.ReminderType == _taskType)), Times.Once);
        }

        [Test]
        public void ThenIfMyRequestIsInvalidIShouldBeNotified()
        {
            //Arrange 
            _validator.Setup(x => x.Validate(It.IsAny<SaveUserReminderSupressionFlagCommand>()))
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
            _repository.Verify(x => x.SaveUserReminderSupression(It.Is<UserReminderSupressionFlag>
                (flag => flag.UserId == _command.UserId &&
                         flag.AccountId == _command.AccountId &&
                         flag.ReminderType == _taskType)), Times.Never);
        }

        [Test]
        public void ThenIShouldBeInformedIfTheSupressionCouldNotBeSaved()
        {
            //Arrange
            _repository.Setup(x =>
                    x.SaveUserReminderSupression(It.IsAny<UserReminderSupressionFlag>()))
                .Throws<Exception>();

            //Act
            Assert.ThrowsAsync<Exception>(async () =>
            {
                await _handler.Handle(_command);
            });
        }
    }
}
