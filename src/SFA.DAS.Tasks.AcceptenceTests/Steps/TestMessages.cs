using SFA.DAS.EmployerAccounts.Events.Messages;

namespace SFA.DAS.Tasks.AcceptenceTests.Steps
{
    public class TestMessages
    {
        public AgreementCreatedMessage AgreementCreated => new AgreementCreatedMessage
        {
            AccountId = 547851,
            LegalEntityId = 8547,
            AgreementId = 9856
        };
    }
}
