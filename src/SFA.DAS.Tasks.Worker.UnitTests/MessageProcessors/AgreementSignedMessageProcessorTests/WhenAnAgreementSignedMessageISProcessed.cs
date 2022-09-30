using System;
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

namespace SFA.DAS.Tasks.Worker.UnitTests.MessageProcessors.AgreementSignedMessageProcessorTests
{
    public class WhenAnAgreementSignedMessageIsProcessed
    {
        private Mock<IMessageSubscriberFactory> _subscriptionFactory;
        private Mock<IMessageSubscriber<AgreementSignedMessage>> _subscriber;
        private AgreementSignedMessage _messageContent;
        private Mock<IMessage<AgreementSignedMessage>> _mockMessage;
        private Mock<IMediator> _mediator;
        private CancellationTokenSource _tokenSource;
        private SignedEmployerAgreementMessageProcessor _processor;

        [SetUp]
        public void Arrange()
        {
            _subscriptionFactory = new Mock<IMessageSubscriberFactory>();
            _subscriber = new Mock<IMessageSubscriber<AgreementSignedMessage>>();

            _messageContent = new AgreementSignedMessage();
         

            _mockMessage = new Mock<IMessage<AgreementSignedMessage>>();
            _mockMessage.Setup(x => x.Content).Returns(_messageContent);

            _mediator = new Mock<IMediator>();
            _tokenSource = new CancellationTokenSource();

            _processor = new SignedEmployerAgreementMessageProcessor(_subscriptionFactory.Object, Mock.Of<ILog>(),
                Mock.Of<IMessageContextProvider>(), _mediator.Object);

            _subscriptionFactory.Setup(x => x.GetSubscriber<AgreementSignedMessage>()).Returns(_subscriber.Object);

            _subscriber.Setup(x => x.ReceiveAsAsync())
                .ReturnsAsync(() => _mockMessage.Object)
                .Callback(() => { _tokenSource.Cancel(); });
        }

        [Test]
        public async Task ThenTheSignAgreementTaskTaskIsCompleted()
        {
            //Act
            await _processor.RunAsync(_tokenSource.Token);

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<SaveTaskCommand>(cmd => cmd.EmployerAccountId.Equals(_messageContent.AccountId.ToString()) &&
                                                                            cmd.Type.Equals(TaskType.AgreementToSign) &&
                                                                            cmd.TaskCompleted.Equals(true))), Times.Once);
        }

        [Test]
        public async Task ThenTheAddAppreticesTaskIsSaved()
        {

            //Act
            await _processor.RunAsync(_tokenSource.Token);

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<SaveTaskCommand>(cmd => cmd.EmployerAccountId.Equals(_messageContent.AccountId.ToString()) &&
                                                                            cmd.Type.Equals(TaskType.AddApprentices) &&
                                                                            cmd.TaskCompleted.Equals(false))), Times.Once);
        }

        [Test]
        public async Task ThenTheAddAppreticesTaskIsNotSavedIfCohortsHaveAlreadyBeenAdded()
        {
            //Arrange
            _messageContent = new AgreementSignedMessage(12,2, "ACME Fireworks", 34,true,"Test User", "ABC123");
            _mockMessage.Setup(x => x.Content).Returns(_messageContent);

            //Act
            await _processor.RunAsync(_tokenSource.Token);

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<SaveTaskCommand>(cmd => cmd.Type.Equals(TaskType.AddApprentices))), Times.Never());
        }

        [Test]
        public async Task ThenAllTasksWillBeSavedIfAnyFail()
        {
            //Arrange
            _mediator.Setup(x => x.SendAsync(It.IsAny<SaveTaskCommand>())).ThrowsAsync(new Exception());
            
            //Act
            await _processor.RunAsync(_tokenSource.Token);

            //Assert
            _mediator.Verify(x => x.SendAsync(It.IsAny<SaveTaskCommand>()), Times.Exactly(2));
        }
    }
}
