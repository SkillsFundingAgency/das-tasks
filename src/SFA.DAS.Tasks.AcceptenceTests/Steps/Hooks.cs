using TechTalk.SpecFlow;
using BoDi;
using StructureMap;
using SFA.DAS.Tasks.API.Client;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Tasks.AcceptenceTests.DependencyResolution;
using SFA.DAS.Tasks.AcceptenceTests.Repository;

namespace SFA.DAS.Tasks.AcceptenceTests.Steps
{
    [Binding]
    public class Hooks
    {
        private IContainer _container;
        private IObjectContainer _objectContainer;

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
            var messagepublisher = _container.GetInstance<IAzureTopicMessageBus>();
            _objectContainer.RegisterInstanceAs(messagepublisher);
            var taskApiClient = _container.GetInstance<ITaskApiClient>();
            _objectContainer.RegisterInstanceAs(taskApiClient);
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            var taskdb = _container.GetInstance<ITaskRepository>();
            var agreement = _objectContainer.Resolve<AgreementCreatedMessage>();
            await taskdb.WithConnection(async c =>
            {
                await c.ExecuteAsync(
                    sql: $"delete from tasks.Tasks where OwnerId = {(int)agreement.AccountId}",
                    commandType: CommandType.Text);
            });
        }
    }
}
