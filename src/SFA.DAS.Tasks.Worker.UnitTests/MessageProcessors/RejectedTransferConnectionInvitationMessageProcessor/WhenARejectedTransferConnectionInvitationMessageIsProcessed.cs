using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.EmployerFinance.Events.Messages;

namespace SFA.DAS.Tasks.Worker.UnitTests.MessageProcessors.RejectedTransferConnectionInvitationMessageProcessor
{
    public class WhenARejectedTransferConnectionInvitationMessageIsProcessed
    {
        private Mock<IMessageSubscriberFactory> _subscriptionFactory;
        private Mock<IMessageSubscriber<RejectedTransferConnectionInvitationEvent>> _subscriber;
        private RejectedTransferConnectionInvitationEvent _messageContent;
        private Mock<IMessage<RejectedTransferConnectionInvitationEvent>> _mockMessage;
        private Mock<IMediator> _mediator;
        private CancellationTokenSource _tokenSource;
        private Worker.MessageProcessors.RejectedTransferConnectionInvitationMessageProcessor _processor;

        [SetUp]
        public void Arrange()
        {
            _subscriptionFactory = new Mock<IMessageSubscriberFactory>();
            _subscriber = new Mock<IMessageSubscriber<RejectedTransferConnectionInvitationEvent>>();

            _messageContent = new RejectedTransferConnectionInvitationEvent();
         
            _mockMessage = new Mock<IMessage<RejectedTransferConnectionInvitationEvent>>();
            _mockMessage.Setup(x => x.Content).Returns(_messageContent);

            _mediator = new Mock<IMediator>();
            _tokenSource = new CancellationTokenSource();

            _processor = new Worker.MessageProcessors.RejectedTransferConnectionInvitationMessageProcessor(_subscriptionFactory.Object, Mock.Of<ILog>(),
                Mock.Of<IMessageContextProvider>(), _mediator.Object);

            _subscriptionFactory.Setup(x => x.GetSubscriber<RejectedTransferConnectionInvitationEvent>()).Returns(_subscriber.Object);

            _subscriber.Setup(x => x.ReceiveAsAsync())
                .ReturnsAsync(() => _mockMessage.Object)
                .Callback(() =>  _tokenSource.Cancel() );
        }


        [Test]
        public async Task ThenTheTaskIsCompleted()
        {
            //Act
            await _processor.RunAsync(_tokenSource.Token);

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<SaveTaskCommand>(cmd => cmd.EmployerAccountId.Equals(_messageContent.ReceiverAccountId.ToString()) &&
                                                                            cmd.Type.Equals(TaskType.ReviewConnectionRequest) &&
                                                                            cmd.TaskCompleted.Equals(true))), Times.Once);
        }
    }
}
