using Microsoft.Azure;
using Microsoft.Owin.Security.ActiveDirectory;
using Owin;
using System;
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
                       ValidAudience = ConfigurationManager.AppSettings["idaAudience"],
                       RoleClaimType = "roles"
                   },
                   Tenant = ConfigurationManager.AppSettings["idaTenant"]
               });
        }
    }
}