using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Tasks.Domain.Configurations
{
    public class FunctionsConfiguration : ITaskConfiguration
    {
        public string DatabaseConnectionString { get; set; }
        public string ServiceBusConnectionString { get; set; }
        public string MessageServiceBusConnectionString { get; set; }
        public Dictionary<string, string> MessageServiceBusConnectionStringLookup { get; set; }
    }
}
