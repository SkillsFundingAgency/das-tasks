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

namespace SFA.DAS.Tasks.Worker.UnitTests.MessageProcessors.LegalEntityRemovedMessageProcessorTests
{
    public class WhenIRemoveALegalEntity
    {
        private Mock<IMessageSubscriberFactory> _subscriberFactory;
        private LegalEntityRemovedMessageProcessor _processor;
        private Mock<ILog> _logger;
        private Mock<IMediator> _mediator;
        private Mock<IMessageSubscriber<LegalEntityRemovedMessage>> _subscriber;
        private CancellationTokenSource _cancellationTokenSource;

        [SetUp]
        public void Arrange()
        {
            _subscriberFactory = new Mock<IMessageSubscriberFactory>();
            _subscriber = new Mock<IMessageSubscriber<LegalEntityRemovedMessage>>();
            _logger = new Mock<ILog>();
            _mediator = new Mock<IMediator>();
            _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            _subscriberFactory.Setup(x => x.GetSubscriber<LegalEntityRemovedMessage>())
                .Returns(_subscriber.Object);

            _processor = new LegalEntityRemovedMessageProcessor(_subscriberFactory.Object, _logger.Object, _mediator.Object);
        }

        [Test]
        public async Task ThenIfTheAgreementHasNotBeenSignedTheRelatedTaskIsRemoved()
        {
            //Arrange
            const long accountId = 12L;
            var message = new Mock<IMessage<LegalEntityRemovedMessage>>();

            message.Setup(x => x.Content)
                .Returns(new LegalEntityRemovedMessage(accountId, 123, false, 345, "Test Org", "Test User", "ABC123"))
                .Callback(_cancellationTokenSource.Cancel);

            _subscriber.Setup(x => x.ReceiveAsAsync()).ReturnsAsync(message.Object);

            //Act
            await _processor.RunAsync(_cancellationTokenSource);

            //Assert
            _mediator.Verify(x => x.SendAsync(It.Is<SaveTaskCommand>(t => t.TaskCompleted && 
                                                                          t.Type == TaskType.AgreementToSign &&
                                                                          t.EmployerAccountId.Equals(accountId.ToString()))), Times.Once);
        }

        [Test]
        public async Task ThenIfTheAgreementHasBeenSignedNoRelatedTaskIsRemoved()
        {
            //Arrange
            var message = new Mock<IMessage<LegalEntityRemovedMessage>>();

            message.Setup(x => x.Content)
                .Returns(new LegalEntityRemovedMessage(12, 123, true, 345, "Test Org", "Test User", "ABC123"))
                .Callback(_cancellationTokenSource.Cancel);

            _subscriber.Setup(x => x.ReceiveAsAsync()).ReturnsAsync(message.Object);

            //Act
            await _processor.RunAsync(_cancellationTokenSource);

            //Assert
            _subscriber.Verify(x => x.ReceiveAsAsync(), Times.Once);
            _mediator.Verify(x => x.SendAsync(It.IsAny<SaveTaskCommand>()), Times.Never);
        }
    }
}
