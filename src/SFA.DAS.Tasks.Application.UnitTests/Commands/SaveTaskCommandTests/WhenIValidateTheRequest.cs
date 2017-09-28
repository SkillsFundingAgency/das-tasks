﻿using System;
using NUnit.Framework;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.Application.Validation;
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
            var request = new SaveTaskCommand { OwnerId = "1234", Type = TaskType.AddApprentices, TaskCompleted = true};

            //Act
            var result = GetResultFromValidator(request);

            //Assert
            Assert.IsTrue(result.IsValid());
        }

        [Test]
        public void ThenIShouldPassValidationWithAValidRequest()
        {
            //Arrange
            var request = new SaveTaskCommand { OwnerId = "1234", Type = TaskType.AddApprentices };

            //Act
            var result = GetResultFromValidator(request);

            //Assert
            Assert.IsTrue(result.IsValid());
        }

        [Test]
        public void ThenIShouldFailValidationIfOwnerIdIsNotPresent()
        {
            //Arrange
            var request = new SaveTaskCommand { Type = TaskType.AddApprentices, TaskCompleted = true };

            //Act
            var result = GetResultFromValidator(request);

            //Assert
            result.VerifyResultIsFalse("Cannot save task when owner ID is not given.",
                result.ValidationDictionary[nameof(request.OwnerId)]);
        }

        [Test]
        public void ThenIShouldFailValidationIfTaskTypeIsNotPresent()
        {
            //Arrange
            var request = new SaveTaskCommand {OwnerId = "1234", TaskCompleted = true};

            //Act
            var result = GetResultFromValidator(request);

            //Assert
            result.VerifyResultIsFalse("Cannot save task when task type is not given.",
                result.ValidationDictionary[nameof(SaveTaskCommand.Type)]);
        }

        private ValidationResult GetResultFromValidator(SaveTaskCommand request)
        {
            var result = _validator.Validate(request);
            return result;
        }
    }
}
