using System;
using System.Configuration;
using Microsoft.Azure;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Configuration.FileStorage;

namespace SFA.DAS.Tasks.Worker.Configuration
{
    public static class ConfigurationHelper
    {
        public static T GetConfiguration<T>(string serviceName)
        {
            var environment = Environment.GetEnvironmentVariable(Constants.DasEnvKeyName);
            if (string.IsNullOrEmpty(environment))
            {
                environment = CloudConfigurationManager.GetSetting(Constants.EnvironmentNameKeyName);
            }

            var configurationRepository = GetConfigurationRepository();
            var configurationService = new ConfigurationService(configurationRepository,
                new ConfigurationOptions(serviceName, environment, Constants.EnvironmentVersionNumber));

            return configurationService.Get<T>();
        }

        private static IConfigurationRepository GetConfigurationRepository()
        {
            IConfigurationRepository configurationRepository;
            if (bool.Parse(ConfigurationManager.AppSettings[Constants.LocalConfigKeyName]))
            {
                configurationRepository = new FileStorageConfigurationRepository();
            }
            else
            {
                configurationRepository = new AzureTableStorageConfigurationRepository(CloudConfigurationManager.GetSetting(Constants.ConfigurationStorageConnectionStringKeyName));
            }
            return configurationRepository;
        }
    }
}
