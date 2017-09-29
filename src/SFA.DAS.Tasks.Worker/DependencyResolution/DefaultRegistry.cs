using MediatR;
using SFA.DAS.Messaging;
using SFA.DAS.NLog.Logger;
using StructureMap;

namespace SFA.DAS.Tasks.Worker.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(scan =>
            {
                scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("SFA.DAS."));
                scan.RegisterConcreteTypesAgainstTheFirstInterface();
                scan.AddAllTypesOf<IMessageProcessor>();
            });

            AddMediatrRegistrations();
            RegisterLogger();
        }

        private void AddMediatrRegistrations()
        {
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));

            For<IMediator>().Use<Mediator>();
        }

        private void RegisterLogger()
        {
            For<ILog>().Use(x => new NLogLogger(
                x.ParentType,
                null,
                null)).AlwaysUnique();
        }
    }
}


