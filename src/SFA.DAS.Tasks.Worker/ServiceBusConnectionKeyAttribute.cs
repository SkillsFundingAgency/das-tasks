using System;

namespace SFA.DAS.Tasks.Worker
{
    public class ServiceBusConnectionKeyAttribute : Attribute
    {
        public ServiceBusConnectionKeyAttribute(string connectionKey)
        {
                
        }
    }
}