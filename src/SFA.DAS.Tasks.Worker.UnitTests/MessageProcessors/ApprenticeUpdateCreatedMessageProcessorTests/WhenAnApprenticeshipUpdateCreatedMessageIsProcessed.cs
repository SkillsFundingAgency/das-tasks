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

namespace SFA.DAS.Tasks.Worker.UnitTests.MessageProcessors.ApprenticeUpdateCreatedMessageProcessorTests
{
    public class WhenAnApprenticeshipUpdateCreatedMessageIsProcessed
    {
        private Mock<IMessageSubscriberFactory> _subscriptionFactory;
        private Mock<IMessageSubscriber<ApprenticeshipUpdateCreated>> _subscriber;
        private ApprenticeshipUpdateCreated _messageContent;
        private Mock<IMessage<ApprenticeshipUpdateCreated>> _mockMessage;
        private Mock<IMediator> _mediator;
        private CancellationTokenSource _tokenSource;
        private ApprenticeshipUpdateCreatedMessageProcessor _processor;

        [SetUp]
        public void Arrange()
        {
            _subscriptionFactory = new Mock<IMessageSubscriberFactory>();
            _subscriber = new Mock<IMessageSubscriber<ApprenticeshipUpdateCreated>>();

            _messageContent = new ApprenticeshipUpdateCreated();

            _mockMessage = new Mock<IMessage<ApprenticeshipUpdateCreated>>();

            _mockMessage.Setup(x => x.Content).Returns(_messageContent);

            _mediator = new Mock<IMediator>();
            _tokenSource = new CancellationTokenSource();

            _processor = new ApprenticeshipUpdateCreatedMessageProcessor(_subscriptionFactory.Object, Mock.Of<ILog>(),
                _mediator.Object);

            _subscriptionFactory.Setup(x => x.GetSubscriber<ApprenticeshipUpdateCreated>()).Returns(_subscriber.Object);

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
            _mediator.Verify(x => x.SendAsync(It.Is<SaveTaskCommand>(cmd => cmd.EmployerAccountId.Equals(_messageContent.AccountId.ToString()) &&
                                                                            cmd.Type.Equals(TaskType.ApprenticeChangesToReview) &&
                                                                            cmd.TaskCompleted.Equals(false))), Times.Once);
        }
    }
}
