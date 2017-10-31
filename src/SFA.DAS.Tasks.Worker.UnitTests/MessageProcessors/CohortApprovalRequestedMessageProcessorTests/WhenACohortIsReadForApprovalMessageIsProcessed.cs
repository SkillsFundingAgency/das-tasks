using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Worker.MessageProcessors;

namespace SFA.DAS.Tasks.Worker.UnitTests.MessageProcessors.CohortApprovalRequestedMessageProcessorTests
{
    public class WhenACohortIsReadForApprovalMessageIsProcessed
    {
        private Mock<IMessageSubscriberFactory> _subscriptionFactory;
        private Mock<IMessageSubscriber<CohortApprovalRequestedByProvider>> _subscriber;
        private CohortApprovalRequestedByProvider _messageContent;
        private Mock<IMessage<CohortApprovalRequestedByProvider>> _mockMessage;
        private Mock<IMediator> _mediator;
        private CancellationTokenSource _tokenSource;
        private CohortApprovalRequestedMessageProcessor _processor;

        [SetUp]
        public void Arrange()
        {
            _subscriptionFactory = new Mock<IMessageSubscriberFactory>();
            _subscriber = new Mock<IMessageSubscriber<CohortApprovalRequestedByProvider>>();

            _messageContent = new CohortApprovalRequestedByProvider();

            _mockMessage = new Mock<IMessage<CohortApprovalRequestedByProvider>>();

            _mockMessage.Setup(x => x.Content).Returns(_messageContent);

            _mediator = new Mock<IMediator>();
            _tokenSource = new CancellationTokenSource();

            _processor = new CohortApprovalRequestedMessageProcessor(_subscriptionFactory.Object, Mock.Of<ILog>(),
                _mediator.Object);

            _subscriptionFactory.Setup(x => x.GetSubscriber<CohortApprovalRequestedByProvider>()).Returns(_subscriber.Object);

            _subscriber.Setup(x => x.ReceiveAsAsync())
                .ReturnsAsync(() => _mockMessage.Object)
                .Callback(() => { _tokenSource.Cancel(); });
        }

        [Test]
        public async Task ThenTheTaskIsSaved()
        {
            //Act
            await _processor.RunAsync(_tokenSource.Token);

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<SaveTaskCommand>(cmd => cmd.OwnerId.Equals(_messageContent.AccountId.ToString()) &&
                                                                            cmd.Type.Equals(TaskType.CohortRequestReadyForApproval) &&
                                                                            cmd.TaskCompleted.Equals(false))), Times.Once);
        }
    }
}
