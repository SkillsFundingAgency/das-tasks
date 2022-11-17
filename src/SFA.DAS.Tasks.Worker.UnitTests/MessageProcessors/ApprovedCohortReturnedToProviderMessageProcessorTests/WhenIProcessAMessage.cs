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

namespace SFA.DAS.Tasks.Worker.UnitTests.MessageProcessors.ApprovedCohortReturnedToProviderMessageProcessorTests
{
    public class WhenIProcessAMessage
    {
        private ApprovedCohortReturnedToProviderMessageProcessor _processor;
        private Mock<IMessageSubscriberFactory> _subscriptionFactory;
        private Mock<IMessageSubscriber<ApprovedCohortReturnedToProvider>> _subscriber;
        private CancellationTokenSource _tokenSource;
        private Mock<IMediator> _mediator;
        private Mock<IMessage<ApprovedCohortReturnedToProvider>> _mockMessage;
        private ApprovedCohortReturnedToProvider _messageContent;

        [SetUp]
        public void Arrange()
        {
            _subscriptionFactory = new Mock<IMessageSubscriberFactory>();
            _subscriber = new Mock<IMessageSubscriber<ApprovedCohortReturnedToProvider>>();

            _messageContent = new ApprovedCohortReturnedToProvider(123, 456, 789);

            _mockMessage = new Mock<IMessage<ApprovedCohortReturnedToProvider>>();

            _mockMessage.Setup(x => x.Content).Returns(_messageContent);

            _mediator = new Mock<IMediator>();
            _tokenSource = new CancellationTokenSource();

            _processor = new ApprovedCohortReturnedToProviderMessageProcessor(_subscriptionFactory.Object, Mock.Of<ILog>(),
                Mock.Of<IMessageContextProvider>(), _mediator.Object);

            _subscriptionFactory.Setup(x => x.GetSubscriber<ApprovedCohortReturnedToProvider>()).Returns(_subscriber.Object);

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
                                                                            cmd.Type.Equals(TaskType.CohortRequestReadyForApproval) &&
                                                                            cmd.TaskCompleted.Equals(true))), Times.Once);
        }
    }
}
