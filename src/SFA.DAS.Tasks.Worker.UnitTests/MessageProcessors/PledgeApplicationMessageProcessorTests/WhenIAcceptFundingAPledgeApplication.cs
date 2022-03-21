using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Messages.Legacy;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.Worker.MessageProcessors;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Tasks.Worker.UnitTests.MessageProcessors.PledgeApplicationMessageProcessorTests
{
    public class WhenIAcceptFundingAPledgeApplication
    {
        private PledgeApplicationFundingAcceptedMessageProcessor _processor;
        private Mock<IMessageSubscriberFactory> _subscriptionFactory;
        private Mock<IMessageSubscriber<PledgeApplicationFundingAccepted>> _subscriber;
        private CancellationTokenSource _tokenSource;
        private Mock<IMediator> _mediator;
        private Mock<IMessage<PledgeApplicationFundingAccepted>> _mockMessage;
        private PledgeApplicationFundingAccepted _messageContent;

        [SetUp]
        public void Arrange()
        {
            _subscriptionFactory = new Mock<IMessageSubscriberFactory>();
            _subscriber = new Mock<IMessageSubscriber<PledgeApplicationFundingAccepted>>();

            _messageContent = new PledgeApplicationFundingAccepted(1, 2, 3, DateTime.UtcNow);

            _mockMessage = new Mock<IMessage<PledgeApplicationFundingAccepted>>();

            _mockMessage.Setup(x => x.Content).Returns(_messageContent);

            _mediator = new Mock<IMediator>();
            _tokenSource = new CancellationTokenSource();

            _processor = new PledgeApplicationFundingAcceptedMessageProcessor(_subscriptionFactory.Object, Mock.Of<ILog>(),
                _mediator.Object);

            _subscriptionFactory.Setup(x => x.GetSubscriber<PledgeApplicationFundingAccepted>()).Returns(_subscriber.Object);

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
            _mediator.Verify(x => x.SendAsync(It.Is<SaveTaskCommand>(cmd =>
                cmd.EmployerAccountId.Equals(_messageContent.AccountId.ToString()) &&
                cmd.Type.Equals(TaskType.PledgeApplicationForAcceptance) &&
                cmd.TaskCompleted.Equals(true))), Times.Once);
        }
    }
}
