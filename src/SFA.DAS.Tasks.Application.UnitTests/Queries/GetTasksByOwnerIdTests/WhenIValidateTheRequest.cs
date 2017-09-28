using NUnit.Framework;
using SFA.DAS.Tasks.Application.Queries.GetTasksByOwnerId;
using SFA.DAS.Tasks.Application.Validation;

namespace SFA.DAS.Tasks.Application.UnitTests.Queries.GetTasksByOwnerIdTests
{
    public class WhenIValidateTheRequest
    {
        private GetTasksByOwnerIdValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new GetTasksByOwnerIdValidator();
        }

        [Test]
        public void ThenIShouldPassValidationWithAValidRequest()
        {
            //Arrange
            var request = new GetTasksByOwnerIdRequest {OwnerId = "1233"};

            //Act
            var result = ResultFromValidator(request);

            //Assert
            Assert.IsTrue(result.IsValid());
        }

        private ValidationResult ResultFromValidator(GetTasksByOwnerIdRequest request)
        {
            var result = _validator.Validate(request);
            return result;
        }

        [Test]
        public void ThenIShouldFailValidationIfOwnerIdIsNotPresent()
        {
            //Arrange
            var request = new GetTasksByOwnerIdRequest();

            //Act
            var result = ResultFromValidator(request);

            //Assert
            Assert.IsFalse(result.IsValid());
            Assert.AreEqual("Cannot get tasks when owner ID is not given.", result.ValidationDictionary[nameof(request.OwnerId)]);
        }
    }
}
