﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.FileSystem;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Worker.MessageHandlers;
using SFA.DAS.Tasks.Worker.Processors;

namespace SFA.DAS.Tasks.Worker.UnitTests.Processors.TaskMessageProcessorTests
{
    public class WhenIProcessAMessage
    {
        private TaskMessageProcessor<TestMessage> _processor;
        private Mock<IPollingMessageReceiver> _messageReciever;
        private Mock<ILog> _logger;
        private Mock<IMessageHandler<TestMessage>> _messageHandler;
        private TestMessage _message;


        [SetUp]
        public void Arrange()
        {
            _messageReciever = new Mock<IPollingMessageReceiver>();
            _logger = new Mock<ILog>();
            _messageHandler = new Mock<IMessageHandler<TestMessage>>();
            _message = new TestMessage();

            _processor = new TaskMessageProcessor<TestMessage>(_messageReciever.Object, _messageHandler.Object, _logger.Object);

            _messageReciever.Setup(x => x.ReceiveAsAsync<TestMessage>())
                .ReturnsAsync(() => new FileSystemMessage<TestMessage>(null, null, _message));
        }

        [Test]
        public async Task ThenTheMessageShouldBeHandledByAHandler()
        {
            //Act
            await _processor.RunAsync(new CancellationToken());

            //Assert
            _messageHandler.Verify(x => x.Handle(_message), Times.Once);
        }

        [Test]
        public async Task ThenIfAErrorOccursTheIssueShouldBeLogged()
        {
            //Arrange
            _messageHandler.Setup(x => x.Handle(It.IsAny<TestMessage>())).Throws<InvalidOperationException>();

            //Act
            await _processor.RunAsync(new CancellationToken());

            //Assert
            _messageHandler.Verify(x => x.Handle(_message), Times.Once);
        }

        private class TestMessage
        { }


}
}
