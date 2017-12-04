using System.Threading;
using TechTalk.SpecFlow;
using BoDi;
using System.Threading.Tasks;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Tasks.API.Client;
using NUnit.Framework;
using System.Linq;
using Microsoft.ServiceBus.Messaging;
using SFA.DAS.Tasks.AcceptenceTests.DependencyResolution;

namespace SFA.DAS.Tasks.AcceptenceTests.Steps
{
    [Binding]
    public class AddOrganisationSteps
    {
        private IObjectContainer _objectContainer;
        private IAzureTopicMessageBus _azureTopicMessageBus;
        private ITaskApiClient _taskApiClient;

        public AddOrganisationSteps(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
            _azureTopicMessageBus = _objectContainer.Resolve<IAzureTopicMessageBus>();
            _taskApiClient = _objectContainer.Resolve<ITaskApiClient>();
        }

            
        [Given(@"I Create an Account or add an Organisation")]
        public void GivenICreateAnAccountOrAddAnOrganisation()
        {
            var agreement = new TestMessages().AgreementCreated;
            _objectContainer.RegisterInstanceAs(agreement);
        }

        [When(@"(agreement_created) message get publish")]
        public async Task WhenAgreement_CreatedMessageGetPublish(string message)
        {
            await Publish<AgreementCreatedMessage>(message);
            int count = 0;
            while (true)
            {
                Thread.Sleep(5000);
                var messageProcessed = await Peek<AgreementCreatedMessage>(message);
                if (messageProcessed == null || count >= 4)
                {
                    break;
                }
                count++;
            }
        }

        [Then(@"I should have a (AgreementToSign) Task")]
        public void ThenIShouldHaveAAgreementToSignTask(string tasktype)
        {
            var agreement = _objectContainer.Resolve<AgreementCreatedMessage>();
            var tasks = _taskApiClient.GetTasks(agreement.AccountId.ToString()).Result.ToList();
            Assert.AreEqual(tasktype, tasks.FirstOrDefault(x => x.OwnerId == agreement.AccountId.ToString())?.Type);
        }

        private async Task Publish<T>(string message)
        {
            var agreement = _objectContainer.Resolve<T>();
            await _azureTopicMessageBus.PublishAsync(agreement);
        }

        private async Task<BrokeredMessage> Peek<T>(string message)
        {
            var agreement = _objectContainer.Resolve<T>();
            return await _azureTopicMessageBus.PeekAsync(agreement);
        }
    }
}
