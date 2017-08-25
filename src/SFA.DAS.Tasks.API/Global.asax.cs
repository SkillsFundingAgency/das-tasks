using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure;
using System.Web.Http;
using System.Web.Mvc;

namespace SFA.DAS.Tasks.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            TelemetryConfiguration.Active.InstrumentationKey = CloudConfigurationManager.GetSetting("InstrumentationKey");
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}
