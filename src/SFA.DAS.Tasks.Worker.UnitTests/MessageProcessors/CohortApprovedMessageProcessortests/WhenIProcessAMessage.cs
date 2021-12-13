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

namespace SFA.DAS.Tasks.Worker.UnitTests.MessageProcessors.CohortApprovedMessageProcessortests
{
    public class WhenIProcessAMessage
    {
        private CohortApprovedMessageProcessor _processor;
        private Mock<IMessageSubscriberFactory> _subscriptionFactory;
        private Mock<IMessageSubscriber<CohortApprovedByEmployer>> _subscriber;
        private CancellationTokenSource _tokenSource;
        private Mock<IMediator> _mediator;
        private Mock<IMessage<CohortApprovedByEmployer>> _mockMessage;
        private CohortApprovedByEmployer _messageContent;

        [SetUp]
        public void Arrange()
        {
            _subscriptionFactory = new Mock<IMessageSubscriberFactory>();
            _subscriber = new Mock<IMessageSubscriber<CohortApprovedByEmployer>>();

            _messageContent = new CohortApprovedByEmployer(123, 456, 789);

            _mockMessage = new Mock<IMessage<CohortApprovedByEmployer>>();

            _mockMessage.Setup(x => x.Content).Returns(_messageContent);

            _mediator = new Mock<IMediator>();
            _tokenSource = new CancellationTokenSource();

            _processor = new CohortApprovedMessageProcessor(_subscriptionFactory.Object, Mock.Of<ILog>(), 
                _mediator.Object);

            _subscriptionFactory.Setup(x => x.GetSubscriber<CohortApprovedByEmployer>()).Returns(_subscriber.Object);

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
