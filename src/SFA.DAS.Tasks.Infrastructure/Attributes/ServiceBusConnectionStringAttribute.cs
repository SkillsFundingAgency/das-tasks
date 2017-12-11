using System;

namespace SFA.DAS.Tasks.Infrastructure.Attributes
{
    public class ServiceBusConnectionStringAttribute : Attribute
    {
        public string Name { get; }

        public ServiceBusConnectionStringAttribute(string name)
        {
            Name = name;
        }
    }
}
