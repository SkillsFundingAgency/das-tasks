using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure;

namespace SFA.DAS.Tasks.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LoggingConfig.ConfigureLogging();

            //TelemetryConfiguration.Active.InstrumentationKey = CloudConfigurationManager.GetSetting("InstrumentationKey");

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
