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

namespace SFA.DAS.Tasks.Worker.UnitTests.MessageProcessors.ApprenticeshipUpdatedAcceptedMessageProcessorTests
{
    public class WhenIProcessAMessage
    {
        private ApprenticeshipUpdateAcceptedMessageProcessor _processor;
        private Mock<IMessageSubscriberFactory> _subscriptionFactory;
        private Mock<IMessageSubscriber<ApprenticeshipUpdateAccepted>> _subscriber;
        private CancellationTokenSource _tokenSource;
        private Mock<IMediator> _mediator;
        private Mock<IMessage<ApprenticeshipUpdateAccepted>> _mockMessage;
        private ApprenticeshipUpdateAccepted _messageContent;

        [SetUp]
        public void Arrange()
        {
            _subscriptionFactory = new Mock<IMessageSubscriberFactory>();
            _subscriber = new Mock<IMessageSubscriber<ApprenticeshipUpdateAccepted>>();

            _messageContent = new ApprenticeshipUpdateAccepted(123, 456, 789);

            _mockMessage = new Mock<IMessage<ApprenticeshipUpdateAccepted>>();

            _mockMessage.Setup(x => x.Content).Returns(_messageContent);

            _mediator = new Mock<IMediator>();
            _tokenSource = new CancellationTokenSource();

            _processor = new ApprenticeshipUpdateAcceptedMessageProcessor(_subscriptionFactory.Object, Mock.Of<ILog>(), 
                _mediator.Object, Mock.Of<IMessageContextProvider>());

            _subscriptionFactory.Setup(x => x.GetSubscriber<ApprenticeshipUpdateAccepted>()).Returns(_subscriber.Object);

            _subscriber.Setup(x => x.ReceiveAsAsync())
                            .ReturnsAsync(() => _mockMessage.Object)
                            .Callback(() => { _tokenSource.Cancel(); });
        }

        [Test]
        public async Task ThenTheMessageShouldBeHandledByAHandler()
        {
            //Act
            await _processor.RunAsync(_tokenSource.Token);

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<SaveTaskCommand>(cmd => cmd.EmployerAccountId.Equals(_messageContent.AccountId.ToString()) &&
                                                                            cmd.Type.Equals(TaskType.ApprenticeChangesToReview) &&
                                                                            cmd.TaskCompleted.Equals(true))), Times.Once);
        }
    }
}
