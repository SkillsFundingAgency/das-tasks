﻿using NUnit.Framework;
using SFA.DAS.Tasks.Application.Queries.GetTask;
using SFA.DAS.Tasks.API.Types.Enums;

namespace SFA.DAS.Tasks.Application.UnitTests.Queries.GetTaskTests
{
    public class WhenIValidateTheRequest
    {
        private GetTaskRequestValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new GetTaskRequestValidator();
        }

        [Test]
        public void ThenIShouldPassValidationWithAValidRequest()
        {
            //Arrange
            var request = new GetTaskRequest {EmployerAccountId = "1233", Type = TaskType.AddApprentices };

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.IsTrue(result.IsValid());
        }

        [Test]
        public void ThenIShouldFailValidationIfEmployerAccountIdIsNotPresent()
        {
            //Arrange
            var request = new GetTaskRequest { Type = TaskType.AddApprentices };

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.IsFalse(result.IsValid());
            Assert.AreEqual("Cannot get task when employer account ID is not given.", result.ValidationDictionary[nameof(request.EmployerAccountId)]);
        }

        [Test]
        public void ThenIShouldFailValidationIfTaskTypeIsNotPresent()
        {
            //Arrange
            var request = new GetTaskRequest { EmployerAccountId = "1233" }; 

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.IsFalse(result.IsValid());
            Assert.AreEqual("Cannot get task when type is not given.", result.ValidationDictionary[nameof(request.Type)]);
        }
    }
}
