using SFA.DAS.Configuration;

namespace SFA.DAS.Tasks.Domain.Configurations
{
    public class TasksConfiguration : IConfiguration
    {
        public string DatabaseConnectionString { get; set; }
        public string ServiceBusConnectionString { get; set; }
    }
}
