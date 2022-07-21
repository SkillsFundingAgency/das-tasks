using Microsoft.Owin.Security.ActiveDirectory;
using Owin;
using System.Configuration;

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
                       ValidAudiences = ConfigurationManager.AppSettings["idaAudience"].Split(','),
                       RoleClaimType = "roles"
                   },
                   Tenant = ConfigurationManager.AppSettings["idaTenant"]
               });
        }
    }
}