using SFA.DAS.Tasks.API.Client;
using TechTalk.SpecFlow;
using BoDi;
using System.Linq;
using SFA.DAS.Tasks.API.Types.DTOs;
using System.Collections.Generic;

namespace SFA.DAS.Tasks.AcceptanceTests.Steps
{
    [Binding]
    public class Assert
    {
        private ITaskApiClient _taskApiClient;
        private IObjectContainer _objectContainer;
        private TestMessages _testMessages;
        private string _employerAccountId;

        public Assert(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
            _taskApiClient = _objectContainer.Resolve<ITaskApiClient>();
            _testMessages = _objectContainer.Resolve<TestMessages>();
            var id = _objectContainer.Resolve<Dictionary<string, object>>("dictionary");
            _employerAccountId = id["employerAccountId"].ToString();
        }

        [Then(@"I should have a (AgreementToSign) Task")]
        public void ThenIShouldHaveAAgreementToSignTask(string tasktype)
        {
            var tasksbytaskstype = TaskDto(tasktype);
            int noofAgreementCreated = _testMessages.NoofAgreementCreated;
            NUnit.Framework.Assert.AreEqual(noofAgreementCreated, tasksbytaskstype?.ItemsDueCount, "AgreementToSign Task is not created");
        }

        [Then(@"I should have a (AddApprentices) Task")]
        public void ThenIShouldHaveAAddApprenticesTask(string tasktype)
        {
            var tasksbytaskstype = TaskDto(tasktype);
            int noofAgreementSigned = _testMessages.NoofAgreementSigned;
            NUnit.Framework.Assert.AreEqual(noofAgreementSigned, tasksbytaskstype?.ItemsDueCount , "AddApprentices Task is not created");
        }

        [Then(@"(AgreementToSign) Task should be removed")]
        public void ThenAgreementToSignTaskShouldBeRemoved(string tasktype)
        {
            var tasksbytaskstype = TaskDto(tasktype);
            int noofAgreementCreated = _testMessages.NoofAgreementCreated;
            NUnit.Framework.Assert.AreEqual(noofAgreementCreated - 1, tasksbytaskstype?.ItemsDueCount, "AgreementToSign Task is not removed");
        }

        [Then(@"(AddApprentices) Task should be removed")]
        public void ThenAddApprenticesTaskShouldBeRemoved(string tasktype)
        {
            var tasksbytaskstype = TaskDto(tasktype);
            NUnit.Framework.Assert.AreEqual(0, tasksbytaskstype?.ItemsDueCount, "AddApprentices Task is not removed");
        }

        [Then(@"(AddApprentices) Task should not be added")]
        public void ThenAddApprenticesTaskShouldNotBeAdded(string tasktype)
        {
            var tasksbytaskstype = TaskDto(tasktype);
            NUnit.Framework.Assert.AreEqual(0, tasksbytaskstype?.ItemsDueCount, "AddApprentices Task is added");
        }


        [Then(@"I should have a (ApprenticeChangesToReview|CohortRequestReadyForApproval) Task")]
        public void ThenIShouldHaveAApprenticeChangesToReviewTask(string tasktype)
        {
            var tasksbytaskstype = TaskDto(tasktype);
            NUnit.Framework.Assert.AreEqual(1, tasksbytaskstype?.ItemsDueCount, $"{tasktype} Task is not created");
        }

        [Then(@"I should have a (2|3) (CohortRequestReadyForApproval) Task")]
        public void ThenIShouldHaveACohortRequestReadyForApprovalTask(int noOfTasks, string tasktype)
        {
            var tasksbytaskstype = TaskDto(tasktype);
            NUnit.Framework.Assert.AreEqual(noOfTasks, tasksbytaskstype?.ItemsDueCount, $"{noOfTasks} times {tasktype} Task is not created");
        }

        [Then(@"(ApprenticeChangesToReview|CohortRequestReadyForApproval) Task should be removed")]
        public void ThenApprenticeChangesToReviewTaskShouldBeRemoved(string tasktype)
        {
            var tasksbytaskstype = TaskDto(tasktype);
            NUnit.Framework.Assert.AreEqual(0, tasksbytaskstype?.ItemsDueCount, $"{tasktype} Task is not removed");
        }

        private TaskDto TaskDto(string tasktype)
        {
            var tasksbyAccountid = _taskApiClient.GetTasks(_employerAccountId, string.Empty).Result.ToList();
            return tasksbyAccountid.FirstOrDefault(x => x.EmployerAccountId == _employerAccountId && x.Type == tasktype);
        }
    }
}
