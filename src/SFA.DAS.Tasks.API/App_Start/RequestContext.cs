using SFA.DAS.NLog.Logger;
using System.Web;

namespace SFA.DAS.Tasks.API
{
    public sealed class RequestContext : ILoggingContext
    {
        public RequestContext(HttpContextBase context)
        {
            HttpMethod = context?.Request.HttpMethod;
            IsAuthenticated = context?.Request.IsAuthenticated;
            Url = context?.Request.Url?.PathAndQuery;
        }

        public string HttpMethod { get; set; }
        public bool? IsAuthenticated { get; set; }
        public string Url { get; }
    }
}