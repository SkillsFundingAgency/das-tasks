using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveUserReminderSuppression;
using SFA.DAS.Tasks.Application.Queries.GetTasksByOwnerId;
using SFA.DAS.Tasks.API.Controllers;
using SFA.DAS.Tasks.API.Types.DTOs;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Domain.Models;

namespace SFA.DAS.Tasks.API.UnitTests.Controllers.TaskControllerTests
{
    public class WhenIDimissATaskReminder
    {
        private const string AccountId = "ABC123";
        private const string UserId = "DEF456";
        private const string TaskType = "AgreementToSign";

        private TaskController _controller;
        private Mock<IMediator> _mediator;
        private List<DasTask> _tasks;

        [SetUp]
        public void Arrange()
        {
            _mediator = new Mock<IMediator>();
            _controller = new TaskController(_mediator.Object, Mock.Of<ILog>());
        }

        [Test]
        public async Task TheDismissShouldBeSaved()
        {
            //Act
            await _controller.AddUserReminderSupression(AccountId, UserId, TaskType);

           //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<SaveUserReminderSuppressionFlagCommand>(
                flag => flag.AccountId.Equals(AccountId) &&
                        flag.UserId.Equals(UserId) &&
                        flag.TaskType.Equals(TaskType))));
        }
    }
}
