using System.Threading;
using TechTalk.SpecFlow;
using BoDi;
using System.Threading.Tasks;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Tasks.AcceptenceTests.DependencyResolution;

namespace SFA.DAS.Tasks.AcceptenceTests.Steps
{
    [Binding]
    public class Act
    {
        private IObjectContainer _objectContainer;
        private IAzureTopicMessageBus _azureTopicMessageBus;

        public Act(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
            _azureTopicMessageBus = _objectContainer.Resolve<IAzureTopicMessageBus>();
        }

        [When(@"(agreement_created|agreement_signed|legal_entity_removed) message get publish")]
        public async Task WhenAgreement_CreatedMessageGetPublish(string message)
        {
            switch (message)
            {
                case "agreement_created":
                    await PublishAndPeak<AgreementCreatedMessage>();
                    break;
                case "agreement_signed":
                    await PublishAndPeak<AgreementSignedMessage>();
                    break;
                case "legal_entity_removed":
                    await PublishAndPeak<LegalEntityRemovedMessage>();
                    break;
            }
        }

        private async Task PublishAndPeak<T>()
        {
            var agreement = _objectContainer.Resolve<T>();
            await _azureTopicMessageBus.PublishAsync(agreement);
            int count = 0;
            while (true)
            {
                await Task.Delay(100);
                var messageProcessed = await _azureTopicMessageBus.PeekAsync(agreement);
                if (messageProcessed == null || count >= 15)
                {
                    break;
                }
                count++;
            }
        }
    }
}
