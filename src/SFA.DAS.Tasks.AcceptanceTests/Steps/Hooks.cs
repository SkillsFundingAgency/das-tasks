using TechTalk.SpecFlow;
using BoDi;
using StructureMap;
using SFA.DAS.Tasks.API.Client;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using SFA.DAS.Tasks.AcceptanceTests.DependencyResolution;
using SFA.DAS.Tasks.AcceptanceTests.Repository;
using System.Linq;
using System;

namespace SFA.DAS.Tasks.AcceptanceTests.Steps
{
    [Binding]
    public class Hooks
    {
        private IContainer _container;
        private IObjectContainer _objectContainer;
        private int noofAgreementCreated;
        private int noofAgreementSigned;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
            _container = new Container(c =>
            {
                c.AddRegistry<TestRegistry>();
            });
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _objectContainer.RegisterInstanceAs(_container.GetInstance<IAzureTopicMessageBus>());
            _objectContainer.RegisterInstanceAs(_container.GetInstance<ITaskApiClient>());
            _objectContainer.RegisterInstanceAs(new TestMessages());
            InitData();
        }

        private void InitData()
        {
            var testMessages = _objectContainer.Resolve<TestMessages>();
            var taskApiClient = _objectContainer.Resolve<ITaskApiClient>();
            CleanData(testMessages.AccountId).Wait();

            // Query to get the state of the Tasks
            var tasks = taskApiClient.GetTasks(testMessages.AccountId.ToString()).Result.ToList();
            noofAgreementCreated = tasks.SingleOrDefault(x => x.Type == "AgreementToSign")?.ItemsDueCount ?? 0;
            noofAgreementSigned = tasks.SingleOrDefault(x => x.Type == "AddApprentices")?.ItemsDueCount ?? 0;
            testMessages.NoofAgreementCreated = noofAgreementCreated;
            testMessages.NoofAgreementSigned = noofAgreementSigned;
        }

        private async Task CleanData(long accountId)
        {
            var taskdb = _container.GetInstance<ITaskRepository>();
            try
            {
                await taskdb.WithConnection(async c =>
                {
                    await c.ExecuteAsync(
                        sql: $"delete from tasks.Tasks where OwnerId = {(int)accountId}",
                        commandType: CommandType.Text);
                });
            }
            catch(Exception)
            {
                //
            }
        }
    }
}
