using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SFA.DAS.Tasks.API.Startup))]

namespace SFA.DAS.Tasks.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
