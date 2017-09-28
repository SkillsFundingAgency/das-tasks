using System.Web;
using MediatR;
using SFA.DAS.NLog.Logger;
using StructureMap;

namespace SFA.DAS.Tasks.API.DependancyResolution
{
    public class DefaultRegistry : Registry
    {
        private const string ServiceNamespace = "SFA.DAS.Tasks";

        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith(ServiceNamespace));
                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });
         
            RegisterMediator();
            RegisterLogger();
        }

        private void RegisterLogger()
        {
            For<IRequestContext>().Use(x => new RequestContext(new HttpContextWrapper(HttpContext.Current)));
            For<ILog>().Use(x => new NLogLogger(
                x.ParentType,
                x.GetInstance<IRequestContext>(),
                null)).AlwaysUnique();
        }

        private void RegisterMediator()
        {
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
            For<IMediator>().Use<Mediator>();
        }
    }
}