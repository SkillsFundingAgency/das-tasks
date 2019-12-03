# Digital Apprenticeships Service
## das-tasks
|               |               |
| ------------- | ------------- |
|![crest](https://assets.publishing.service.gov.uk/government/assets/crests/org_crest_27px-916806dcf065e7273830577de490d5c7c42f36ddec83e907efe62086785f24fb.png)|Tasks|
| Build | [![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/Manage%20Apprenticeships/das-tasks?branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=540&branchName=master) |

## Developer setup
### Requirements
- Visual Studio
- Azure storage account or Azure storage emulator

### Setup local environment
- Open the solution in Visual Studio `admin` mode
- Set SFA.DAS.Tasks.CloudService as the startup project
- Update ConfigurationSettings in [ServiceConfiguration.Local.cscfg](src/SFA.DAS.Tasks.CloudService.API)
  ```
    <ConfigurationSettings>
      <Setting name="EnvironmentName" value="LOCAL" />
      <Setting name="ConfigurationStorageConnectionString" value="UseDevelopmentStorage=true" />
      <Setting name="StorageConnectionString" value="UseDevelopmentStorage=true" />
      <Setting name="LogLevel" value="Debug" />
      <Setting name="LoggingRedisConnectionString" value="" />
      <Setting name="LoggingRedisKey" value="" />
      <Setting name="InstrumentationKey" value=""/>
      <Setting name="idaTenant" value="citizenazuresfabisgov.onmicrosoft.com" />
      <Setting name="idaAudience" value="https://citizenazuresfabisgov.onmicrosoft.com/tasks-api" />
    </ConfigurationSettings>
  ```
- Disable Diagnostics on API and Worker Roles under the CloudService. Expand `Roles` folder in Visual Studio or directly edit `diagnostics.wadcfgx`for both the roles. 

_If you have issues starting on port 443 make sure that the certificates are installed and no other process is running on 443. I found Cosmos emulator uses port 443, this is not required so can be turned off._
