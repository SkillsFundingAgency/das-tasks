using SFA.DAS.EmployerAccounts.Events.Messages;

namespace SFA.DAS.Tasks.AcceptanceTests.Steps
{
    public class TestMessages
    {
        public long AccountId => 547851;

        public int NoofAgreementCreated { get; set; }

        public int NoofAgreementSigned { get; set; }

        public AgreementCreatedMessage AgreementCreated => new AgreementCreatedMessage
        {
            AccountId = AccountId,
            LegalEntityId = 8547,
            AgreementId = 9856
        };
        public AgreementSignedMessage AgreementSigned(long accountId, long legalEntityId, long agreementId)
        {
            return new AgreementSignedMessage
            {
                AccountId = accountId,
                LegalEntityId = legalEntityId,
                AgreementId = agreementId
            };
        }

        public LegalEntityRemovedMessage LegalEntityRemovedMessage(long accountId, long legalEntityId, long agreementId,bool agreementSigned)
        {
            return new LegalEntityRemovedMessage
            {
                AccountId = accountId,
                LegalEntityId = legalEntityId,
                AgreementId = agreementId,
                AgreementSigned = agreementSigned
            };
        }
    }
}
