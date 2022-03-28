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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Tasks.Worker.UnitTests.MessageProcessors.PledgeApplicationMessageProcessorTests
{
    public class WhenIDeclineFundingAPledgeApplication
    {
        private PledgeApplicationFundingDeclinedMessageProcessor _processor;
        private Mock<IMessageSubscriberFactory> _subscriptionFactory;
        private Mock<IMessageSubscriber<PledgeApplicationFundingDeclined>> _subscriber;
        private CancellationTokenSource _tokenSource;
        private Mock<IMediator> _mediator;
        private Mock<IMessage<PledgeApplicationFundingDeclined>> _mockMessage;
        private PledgeApplicationFundingDeclined _messageContent;

        [SetUp]
        public void Arrange()
        {
            _subscriptionFactory = new Mock<IMessageSubscriberFactory>();
            _subscriber = new Mock<IMessageSubscriber<PledgeApplicationFundingDeclined>>();

            _messageContent = new PledgeApplicationFundingDeclined(1, 2, DateTime.UtcNow, 3);

            _mockMessage = new Mock<IMessage<PledgeApplicationFundingDeclined>>();

            _mockMessage.Setup(x => x.Content).Returns(_messageContent);

            _mediator = new Mock<IMediator>();
            _tokenSource = new CancellationTokenSource();

            _processor = new PledgeApplicationFundingDeclinedMessageProcessor(_subscriptionFactory.Object, Mock.Of<ILog>(),
                _mediator.Object);

            _subscriptionFactory.Setup(x => x.GetSubscriber<PledgeApplicationFundingDeclined>()).Returns(_subscriber.Object);

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
