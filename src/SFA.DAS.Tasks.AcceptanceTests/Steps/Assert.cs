using SFA.DAS.Tasks.API.Client;
using TechTalk.SpecFlow;
using BoDi;
using SFA.DAS.EmployerAccounts.Events.Messages;
using System.Linq;
using SFA.DAS.Tasks.API.Types.DTOs;

namespace SFA.DAS.Tasks.AcceptanceTests.Steps
{
    [Binding]
    public class Assert
    {
        private ITaskApiClient _taskApiClient;
        private IObjectContainer _objectContainer;
        private TestMessages _testMessages;
        private string _accountId;

        public Assert(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
            _taskApiClient = _objectContainer.Resolve<ITaskApiClient>();
            _testMessages = _objectContainer.Resolve<TestMessages>();
            _accountId = _testMessages.AccountId.ToString();
        }

        [Then(@"I should have a (AgreementToSign) Task")]
        public void ThenIShouldHaveAAgreementToSignTask(string tasktype)
        {
            var tasksbytaskstype = TaskDto(_accountId, tasktype);
            int noofAgreementCreated = _testMessages.NoofAgreementCreated;
            NUnit.Framework.Assert.AreEqual(noofAgreementCreated, tasksbytaskstype?.ItemsDueCount, "AgreementToSign Task is not created");
        }

        [Then(@"I should have a (AddApprentices) Task")]
        public void ThenIShouldHaveAAddApprenticesTask(string tasktype)
        {
            var tasksbytaskstype = TaskDto(_accountId, tasktype);
            int noofAgreementSigned = _testMessages.NoofAgreementSigned;
            NUnit.Framework.Assert.AreEqual(noofAgreementSigned, tasksbytaskstype?.ItemsDueCount , "AddApprentices Task is not created");
        }

        [Then(@"(AgreementToSign) Task should be removed")]
        public void ThenAgreementToSignTaskShouldBeRemoved(string tasktype)
        {
            var tasksbytaskstype = TaskDto(_accountId, tasktype);
            int noofAgreementCreated = _testMessages.NoofAgreementCreated;
            NUnit.Framework.Assert.AreEqual(noofAgreementCreated - 1, tasksbytaskstype?.ItemsDueCount, "AgreementToSign Task is not removed");
        }

        private TaskDto TaskDto(string accountid, string tasktype)
        {
            var tasksbyAccountid = _taskApiClient.GetTasks(accountid).Result.ToList();
            return tasksbyAccountid.FirstOrDefault(x => x.OwnerId == accountid && x.Type == tasktype);
        }
    }
}
