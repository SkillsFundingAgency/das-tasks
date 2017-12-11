using NUnit.Framework;
using SFA.DAS.Tasks.Application.Queries.GetTasksByEmployerAccountId;

namespace SFA.DAS.Tasks.Application.UnitTests.Queries.GetTasksByEmployerAccountIdTests
{
    public class WhenIValidateTheRequest
    {
        private GetTasksByEmployerAccountIdValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new GetTasksByEmployerAccountIdValidator();
        }

        [Test]
        public void ThenIShouldPassValidationWithAValidRequest()
        {
            //Arrange
            var request = new GetTasksByEmployerAccountIdRequest {EmployerAccountId = "1233"};

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.IsTrue(result.IsValid());
        }

        [Test]
        public void ThenIShouldFailValidationIfEmployerAccountIdIsNotPresent()
        {
            //Arrange
            var request = new GetTasksByEmployerAccountIdRequest();

            //Act
            var result = _validator.Validate(request);

            //Assert
            Assert.IsFalse(result.IsValid());
            Assert.AreEqual("Cannot get tasks when employer account ID is not given.", result.ValidationDictionary[nameof(request.EmployerAccountId)]);
        }
    }
}
