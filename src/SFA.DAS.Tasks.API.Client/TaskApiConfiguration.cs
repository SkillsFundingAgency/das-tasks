namespace SFA.DAS.Tasks.API.Client
{
    public class TaskApiConfiguration : ITaskApiConfiguration
    {
        public string ApiBaseUrl { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string IdentifierUri { get; }
        public string Tenant { get; }
    }
}
