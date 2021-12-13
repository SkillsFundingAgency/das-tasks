using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Worker.MessageProcessors;

namespace SFA.DAS.Tasks.Worker.UnitTests.MessageProcessors.TransferConnectionInvitationSentMessageProcessorTests
{
    public class WhenIProcessAMessage
    {
        private SentTransferConnectionInvitationEventMessageProcessor _processor;
        private Mock<IMessageSubscriberFactory> _subscriptionFactory;
        private Mock<IMessageSubscriber<SentTransferConnectionInvitationEvent>> _subscriber;
        private CancellationTokenSource _tokenSource;
        private Mock<IMediator> _mediator;
        private Mock<IMessage<SentTransferConnectionInvitationEvent>> _mockMessage;
        private SentTransferConnectionInvitationEvent _messageContent;

        [SetUp]
        public void Arrange()
        {
            _subscriptionFactory = new Mock<IMessageSubscriberFactory>();
            _subscriber = new Mock<IMessageSubscriber<SentTransferConnectionInvitationEvent>>();

            _messageContent = new SentTransferConnectionInvitationEvent
            {
                TransferConnectionInvitationId = 123,
                SenderAccountId = 456,
                SenderAccountName = "Sender Account",
                ReceiverAccountId = 789,
                ReceiverAccountName = "Receiver Account",
                SentByUserName = "Bill",
                SenderAccountHashedId = "ABC123"
            };

            _mockMessage = new Mock<IMessage<SentTransferConnectionInvitationEvent>>();

            _mockMessage.Setup(x => x.Content).Returns(_messageContent);

            _mediator = new Mock<IMediator>();
            _tokenSource = new CancellationTokenSource();

            _processor = new Worker.MessageProcessors.SentTransferConnectionInvitationEventMessageProcessor(_subscriptionFactory.Object, Mock.Of<ILog>(), 
                _mediator.Object);

            _subscriptionFactory.Setup(x => x.GetSubscriber<SentTransferConnectionInvitationEvent>()).Returns(_subscriber.Object);

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
            _mediator.Verify(x => x.SendAsync(It.Is<SaveTaskCommand>(cmd => cmd.EmployerAccountId.Equals(_messageContent.ReceiverAccountId.ToString()) &&
                                                                            cmd.Type.Equals(TaskType.ReviewConnectionRequest) &&
                                                                            cmd.TaskCompleted.Equals(false))), Times.Once);
        }
    }
}
