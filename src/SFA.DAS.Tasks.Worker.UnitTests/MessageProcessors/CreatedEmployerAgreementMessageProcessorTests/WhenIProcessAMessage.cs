using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Messaging.FileSystem;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Application.Commands.SaveTask;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Worker.MessageProcessors;

namespace SFA.DAS.Tasks.Worker.UnitTests.MessageProcessors.CreatedEmployerAgreementMessageProcessorTests
{
    public class WhenIProcessAMessage
    {
        private CreatedEmployerAgreementMessageProcessor _processor;
        private Mock<IMessageSubscriberFactory> _subscriptionFactory;
        private Mock<IMessageSubscriber<AgreementCreatedMessage>> _subscriber;
        private CancellationTokenSource _tokenSource;
        private Mock<IMediator> _mediator;
        private Mock<IMessage<AgreementCreatedMessage>> _mockMessage;
        private AgreementCreatedMessage _messageContent;

        [SetUp]
        public void Arrange()
        {
            _subscriptionFactory = new Mock<IMessageSubscriberFactory>();
            _subscriber = new Mock<IMessageSubscriber<AgreementCreatedMessage>>();

            _messageContent = new AgreementCreatedMessage(123, 2, "Test Corp", 456, "Test User", "ABC123");

            _mockMessage = new Mock<IMessage<AgreementCreatedMessage>>();

            _mockMessage.Setup(x => x.Content).Returns(_messageContent);

            _mediator = new Mock<IMediator>();
            _tokenSource = new CancellationTokenSource();

            _processor = new CreatedEmployerAgreementMessageProcessor(_subscriptionFactory.Object, Mock.Of<ILog>(), 
                _mediator.Object);

            _subscriptionFactory.Setup(x => x.GetSubscriber<AgreementCreatedMessage>()).Returns(_subscriber.Object);

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
                                                                            cmd.Type.Equals(TaskType.AgreementToSign) &&
                                                                            cmd.TaskCompleted.Equals(false))), Times.Once);
        }
    }
}
