using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using SFA.DAS.Tasks.Worker.DependencyResolution;
using SFA.DAS.Tasks.Worker.Processors;
using StructureMap;

namespace SFA.DAS.Tasks.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private IContainer _container;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);
      

        public override void Run()
        {
            Trace.TraceInformation("SFA.DAS.Tasks.Worker is running");

            try
            {
                var messageProcessors = _container.GetAllInstances<ITaskMessageProcessor>();

                var tasks = messageProcessors.Select(x => x.RunAsync(_cancellationTokenSource.Token)).ToArray();
                Task.WaitAll(tasks);
            }
            finally
            {
                _runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            var result = base.OnStart();

            Trace.TraceInformation("SFA.DAS.Tasks.Worker has been started");

            _container = new Container(c =>
            {
                c.AddRegistry<DefaultRegistry>();
            });

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("SFA.DAS.Tasks.Worker is stopping");

            _cancellationTokenSource.Cancel();
            _runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("SFA.DAS.Tasks.Worker has stopped");
        }
    }
}
