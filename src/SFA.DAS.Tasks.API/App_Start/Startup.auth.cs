using Microsoft.Azure;
using Microsoft.Owin.Security.ActiveDirectory;
using Owin;

namespace SFA.DAS.Tasks.API
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
               new WindowsAzureActiveDirectoryBearerAuthenticationOptions
               {
                   TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
                   {
                       ValidAudience = CloudConfigurationManager.GetSetting("idaAudience"),
                       RoleClaimType = "roles"
                   },
                   Tenant = CloudConfigurationManager.GetSetting("idaTenant")
               });
        }
    }
}