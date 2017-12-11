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
using System.Collections.Generic;

namespace SFA.DAS.Tasks.AcceptanceTests.Steps
{
    [Binding]
    public class Hooks
    {
        private IContainer _container;
        private IObjectContainer _objectContainer;
        private int noofAgreementCreated;
        private int noofAgreementSigned;
        private Dictionary<string, object> dictionary;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
            dictionary = new Dictionary<string, object>();
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
            var employerAccountId = testMessages.EmployerAccountId;
            dictionary.Add("employerAccountId", employerAccountId);
            _objectContainer.RegisterInstanceAs(dictionary, "dictionary");
            CleanData(employerAccountId).Wait();

            // Query to get the state of the Tasks
            var tasks = taskApiClient.GetTasks(employerAccountId.ToString(), string.Empty).Result.ToList();
            noofAgreementCreated = tasks.SingleOrDefault(x => x.Type == "AgreementToSign")?.ItemsDueCount ?? 0;
            noofAgreementSigned = tasks.SingleOrDefault(x => x.Type == "AddApprentices")?.ItemsDueCount ?? 0;
            testMessages.NoofAgreementCreated = noofAgreementCreated;
            testMessages.NoofAgreementSigned = noofAgreementSigned;
        }

        private async Task CleanData(long employerAccountId)
        {
            var taskdb = _container.GetInstance<ITaskRepository>();
            try
            {
                await taskdb.WithConnection(async c =>
                {
                    await c.ExecuteAsync(
                        sql: $"delete from tasks.Tasks where EmployerAccountId = {(int)employerAccountId}",
                        commandType: CommandType.Text);
                });
                Console.WriteLine($"Delete tasks.Tasks with EmployerAccountId {employerAccountId}.");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
