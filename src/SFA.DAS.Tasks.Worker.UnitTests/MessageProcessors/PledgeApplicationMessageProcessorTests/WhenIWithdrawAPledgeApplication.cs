using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Messages.Legacy;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.Worker.MessageProcessors;

namespace SFA.DAS.Tasks.Worker.UnitTests.MessageProcessors.PledgeApplicationMessageProcessorTests
{
    public class WhenIWithdrawAPledgeApplication
    {
        private PledgeApplicationWithdrawnMessageProcessor _processor;
        private Mock<IMessageSubscriberFactory> _subscriptionFactory;
        private Mock<IMessageSubscriber<PledgeApplicationWithdrawn>> _subscriber;
        private CancellationTokenSource _tokenSource;
        private Mock<IMediator> _mediator;
        private Mock<IMessage<PledgeApplicationWithdrawn>> _mockMessage;
        private PledgeApplicationWithdrawn _messageContent;

        [SetUp]
        public void Arrange()
        {
            _subscriptionFactory = new Mock<IMessageSubscriberFactory>();
            _subscriber = new Mock<IMessageSubscriber<PledgeApplicationWithdrawn>>();

            _messageContent = new PledgeApplicationWithdrawn(1, 2, DateTime.UtcNow, 3);

            _mockMessage = new Mock<IMessage<PledgeApplicationWithdrawn>>();

            _mockMessage.Setup(x => x.Content).Returns(_messageContent);

            _mediator = new Mock<IMediator>();
            _tokenSource = new CancellationTokenSource();

            _processor = new PledgeApplicationWithdrawnMessageProcessor(_subscriptionFactory.Object, Mock.Of<ILog>(),
                Mock.Of<IMessageContextProvider>(), _mediator.Object);

            _subscriptionFactory.Setup(x => x.GetSubscriber<PledgeApplicationWithdrawn>()).Returns(_subscriber.Object);

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
            _mediator.Verify(x => x.SendAsync(It.Is<SaveTaskCommand>(cmd =>
                cmd.EmployerAccountId.Equals(_messageContent.TransferSenderId.ToString()) &&
                cmd.Type.Equals(TaskType.PledgeApplicationForReview) &&
                cmd.TaskCompleted.Equals(true))), Times.Once);
        }
    }
}