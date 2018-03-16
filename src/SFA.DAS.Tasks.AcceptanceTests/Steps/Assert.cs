using SFA.DAS.Tasks.API.Client;
using TechTalk.SpecFlow;
using BoDi;
using System.Linq;
using SFA.DAS.Tasks.API.Types.DTOs;
using System.Collections.Generic;
using Polly;
using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SFA.DAS.Tasks.AcceptanceTests.Steps
{
    [Binding]
    public class Assert
    {
        private ITaskApiClient _taskApiClient;
        private IObjectContainer _objectContainer;
        private TestMessages _testMessages;
        private string _employerAccountId;

        public Assert()
        {
        }

        public Assert(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
            _taskApiClient = _objectContainer.Resolve<ITaskApiClient>();
            _testMessages = _objectContainer.Resolve<TestMessages>();
            var id = _objectContainer.Resolve<Dictionary<string, object>>("dictionary");
            _employerAccountId = id["employerAccountId"].ToString();
        }

        [Then(@"I should have a (AgreementToSign) Task")]
        public async Task ThenIShouldHaveAAgreementToSignTask(string tasktype)
        {
            int noofAgreementCreated = _testMessages.NoofAgreementCreated;
            int count = 0;
            await PollyRetryAsync(async () =>
             {
                 var tasksbytaskstype = await TaskDto(tasktype);
                 count++;
                 NUnit.Framework.Assert.AreEqual(noofAgreementCreated, tasksbytaskstype?.ItemsDueCount, $"AgreementToSign Task is not created, after {count} retry");
             });
        }

        [Then(@"I should have a (AddApprentices) Task")]
        public async Task ThenIShouldHaveAAddApprenticesTask(string tasktype)
        {
            int noofAgreementSigned = _testMessages.NoofAgreementSigned;
            int count = 0;
            await PollyRetryAsync(async () => 
            {
                var tasksbytaskstype = await TaskDto(tasktype);
                count++;
                NUnit.Framework.Assert.AreEqual(noofAgreementSigned, tasksbytaskstype?.ItemsDueCount, $"AddApprentices Task is not created, after {count} retry");
            });
        }

        [Then(@"(AgreementToSign) Task should be removed")]
        public async Task ThenAgreementToSignTaskShouldBeRemoved(string tasktype)
        {
            int noofAgreementCreated = _testMessages.NoofAgreementCreated;
            int count = 0;
            await PollyRetryAsync(async () =>
            {
                var tasksbytaskstype = await TaskDto(tasktype);
                count++;
                NUnit.Framework.Assert.AreEqual(noofAgreementCreated - 1, tasksbytaskstype?.ItemsDueCount, $"AgreementToSign Task is not removed, after {count} retry");
            });      
        }

        [Then(@"(AddApprentices) Task should be removed")]
        public async Task ThenAddApprenticesTaskShouldBeRemoved(string tasktype)
        {
            int count = 0;
            await PollyRetryAsync(async () => 
            {
                var tasksbytaskstype = await TaskDto(tasktype);
                count++;
                NUnit.Framework.Assert.AreEqual(0, tasksbytaskstype?.ItemsDueCount, $"AddApprentices Task is not removed, after {count} retry");
            });            
        }

        [Then(@"(AddApprentices) Task should not be added")]
        public async Task ThenAddApprenticesTaskShouldNotBeAdded(string tasktype)
        {
            int count = 0;
            await PollyRetryAsync(async () =>
            {
                var tasksbytaskstype = await TaskDto(tasktype);
                count++;
                NUnit.Framework.Assert.AreEqual(0, tasksbytaskstype?.ItemsDueCount, $"AddApprentices Task is added, after {count} retry");
            });
        }

        [Then(@"I should have a (ApprenticeChangesToReview|CohortRequestReadyForApproval|ReviewConnectionRequest) Task")]
        public async Task ThenIShouldHaveAApprenticeChangesToReviewTask(string tasktype)
        {
            int count = 0;
            await PollyRetryAsync(async () =>
            {
                var tasksbytaskstype = await TaskDto(tasktype);
                count++;
                NUnit.Framework.Assert.AreEqual(1, tasksbytaskstype?.ItemsDueCount, $"{tasktype} Task is not created, after {count} retry");
            });            
        }

        [Then(@"I should have a (2|3) (CohortRequestReadyForApproval) Task")]
        public async Task ThenIShouldHaveACohortRequestReadyForApprovalTask(int noOfTasks, string tasktype)
        {
            int count = 0;
            await PollyRetryAsync(async () =>
            {
                var tasksbytaskstype = await TaskDto(tasktype);
                count++;
                NUnit.Framework.Assert.AreEqual(noOfTasks, tasksbytaskstype?.ItemsDueCount, $"{tasktype} Task is not displayed {noOfTasks} times, after {count} retry");
            });
        }

        [Then(@"(ApprenticeChangesToReview|CohortRequestReadyForApproval) Task should be removed")]
        public async Task ThenApprenticeChangesToReviewTaskShouldBeRemoved(string tasktype)
        {
            int count = 0;
            await PollyRetryAsync(async () =>
            {
                var tasksbytaskstype = await TaskDto(tasktype);
                count++;
                NUnit.Framework.Assert.AreEqual(0, tasksbytaskstype?.ItemsDueCount, $"{tasktype} Task is not removed, after {count} retry");
            });
        }

        [Then(@"I should not have a (ReviewConnectionRequest) Task")]
        public async Task ThenIShouldNotHaveAApprenticeChangesToReviewTask(string tasktype)
        {
            int count = 0;
            await PollyRetryAsync(async () =>
            {
                var tasksbytaskstype = await TaskDto(tasktype);
                count++;
                NUnit.Framework.Assert.AreEqual(0, tasksbytaskstype?.ItemsDueCount, $"{tasktype} Task is not removed, after {count} retry");

                //NUnit.Framework.Assert.IsNull(tasksbytaskstype, $"{tasktype} Task is still present, after {count} retry");
            });
        }


        private async Task<TaskDto> TaskDto(string tasktype)
        {
            var tasks = await _taskApiClient.GetTasks(_employerAccountId, string.Empty);
            var tasksbyAccountid = tasks.ToList();
            return tasksbyAccountid.FirstOrDefault(x => x.EmployerAccountId == _employerAccountId && x.Type == tasktype);
        }

        public async Task PollyRetryAsync(Func<Task> action)
        {
            await Policy
                .Handle<AssertionException>()
                .WaitAndRetryAsync(new[] { TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5) })
                .ExecuteAsync(action);
        }
    }

    [TestFixture,Ignore("Tests to test the tests")]
    public class TestPollyRetry
    {
        Assert TestAssertStep;

        [OneTimeSetUp]
        public void Setup()
        {
            TestAssertStep = new Assert();
        }

        [Test]
        public void ShouldThrowException()
        {
            Func<Task> action = async () =>
            {
                await Task.Delay(0);
                NUnit.Framework.Assert.AreEqual(3, 2);
            };
            
            NUnit.Framework.Assert.ThrowsAsync<AssertionException>(() => TestAssertStep.PollyRetryAsync(action));
        }

        [Test]
        public void ShouldNotThrowException()
        {
            Func<Task> action = async () =>
            {
                await Task.Delay(0);
                NUnit.Framework.Assert.AreEqual(2, 2);
            };

            NUnit.Framework.Assert.DoesNotThrowAsync(() => TestAssertStep.PollyRetryAsync(action));
        }

        [Test]
        public void ShouldRetryThreeTimes()
        {
            int count = 0;
            Func<Task> action = async () => 
            {
                await Task.Delay(0);
                count++;
                NUnit.Framework.Assert.AreEqual(3, 2);
            };
            NUnit.Framework.Assert.ThrowsAsync<AssertionException>(() => TestAssertStep.PollyRetryAsync(action));

            NUnit.Framework.Assert.AreEqual(3, count);
        }
    }
}

