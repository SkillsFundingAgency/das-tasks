using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using SFA.DAS.Messaging.Interfaces;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Tasks.Domain.Configurations;
using SFA.DAS.Tasks.Infrastructure.DependencyResolution.Configuration;
using SFA.DAS.Tasks.Infrastructure.DependencyResolution.Configuration.Policies;
using SFA.DAS.Tasks.Worker.DependencyResolution;
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

            var logger = _container.GetInstance<ILog>();

            try
            {
                logger.Debug("Getting message processors");

                var messageProcessors = _container.GetAllInstances<IMessageProcessor2>().ToList();

                logger.Debug($"Found {messageProcessors.Count} message processors");

                foreach (var processor in messageProcessors)
                {
                    logger.Debug($"Found message processor of type {processor.GetType().FullName}");
                }

                var tasks = messageProcessors.Select(x => x.RunAsync(_cancellationTokenSource.Token)).ToArray();
                Task.WaitAll(tasks);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Task Worker has had an unhandled excdeption and is exiting");
                throw;
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
                c.Policies.Add(new ConfigurationPolicy<TasksConfiguration>("SFA.DAS.Tasks"));
                c.Policies.Add(new MessagePublisherPolicy<TasksConfiguration>("SFA.DAS.Tasks"));
                c.Policies.Add(new MessageSubscriberPolicy<TasksConfiguration>("SFA.DAS.Tasks"));
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
