using System.Collections.Generic;
using SFA.DAS.Configuration;

namespace SFA.DAS.Tasks.Domain.Configurations
{
    public interface ITaskConfiguration : IConfiguration
    {
        Dictionary<string, string> MessageServiceBusConnectionStringLookup { get; set; }
    }
}
