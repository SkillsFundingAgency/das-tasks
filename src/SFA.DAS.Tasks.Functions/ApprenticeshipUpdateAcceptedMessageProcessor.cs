using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Tasks.API.Types.Enums;
using SFA.DAS.Tasks.Application.Commands.SaveTask;

namespace SFA.DAS.Tasks.Functions
{
    public class ApprenticeshipUpdateAcceptedMessageProcessor
    {
        IConfiguration _configuration;
        IMediator _mediator;

        public ApprenticeshipUpdateAcceptedMessageProcessor(IConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
        }
        
        [FunctionName("ApprenticeshipUpdateAcceptedMessageProcessor")]
        public async Task Run([ServiceBusTrigger("apprenticeship_update_accepted", "Task_ApprenticeshipUpdateAccepted", Connection = "MessageServiceBusConnectionStringLookup_Commitments")] 
            ApprenticeshipUpdateAccepted message, ILogger log)
        {
           log.LogDebug($"Aprretniceship update accepted. Completing 'apprentice changes to review' task for account id {message.AccountId}, " +
           $"apprentice id {message.ApprenticeshipId} and provider id {message.ProviderId}");

            await _mediator.Send(new SaveTaskCommand
            {
                EmployerAccountId = message.AccountId.ToString(),
                Type = TaskType.ApprenticeChangesToReview,
                TaskCompleted = true
            });
        }
    }
}
