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
        private TransferConnectionInvitationSentMessageProcessor _processor;
        private Mock<IMessageSubscriberFactory> _subscriptionFactory;
        private Mock<IMessageSubscriber<TransferConnectionInvitationSentMessage>> _subscriber;
        private CancellationTokenSource _tokenSource;
        private Mock<IMediator> _mediator;
        private Mock<IMessage<TransferConnectionInvitationSentMessage>> _mockMessage;
        private TransferConnectionInvitationSentMessage _messageContent;

        [SetUp]
        public void Arrange()
        {
            _subscriptionFactory = new Mock<IMessageSubscriberFactory>();
            _subscriber = new Mock<IMessageSubscriber<TransferConnectionInvitationSentMessage>>();

            _messageContent = new TransferConnectionInvitationSentMessage(123, 456, "Sender Account", 789, "Receiver Account", "Bill", "ABC123");

            _mockMessage = new Mock<IMessage<TransferConnectionInvitationSentMessage>>();

            _mockMessage.Setup(x => x.Content).Returns(_messageContent);

            _mediator = new Mock<IMediator>();
            _tokenSource = new CancellationTokenSource();

            _processor = new Worker.MessageProcessors.TransferConnectionInvitationSentMessageProcessor(_subscriptionFactory.Object, Mock.Of<ILog>(), 
                _mediator.Object);

            _subscriptionFactory.Setup(x => x.GetSubscriber<TransferConnectionInvitationSentMessage>()).Returns(_subscriber.Object);

            _subscriber.Setup(x => x.ReceiveAsAsync())
                            .ReturnsAsync(() => _mockMessage.Object)
                            .Callback(() => { _tokenSource.Cancel(); });
        }

        [Test]
        public async Task ThenTheMessageShouldBeHandledByAHandler()
        {
            //Act
            await _processor.RunAsync(_tokenSource);

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<SaveTaskCommand>(cmd => cmd.EmployerAccountId.Equals(_messageContent.ReceiverAccountId.ToString()) &&
                                                                            cmd.Type.Equals(TaskType.ReviewConnectionRequest) &&
                                                                            cmd.TaskCompleted.Equals(false))), Times.Once);
        }
    }
}
