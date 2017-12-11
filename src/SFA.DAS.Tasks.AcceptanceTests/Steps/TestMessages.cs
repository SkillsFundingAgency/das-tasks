using SFA.DAS.EmployerAccounts.Events.Messages;
using System;

namespace SFA.DAS.Tasks.AcceptanceTests.Steps
{
    public class TestMessages
    {
        public long EmployerAccountId => GenerateRandomid();

        public int NoofAgreementCreated { get; set; }

        public int NoofAgreementSigned { get; set; }

        private long GenerateRandomid()
        {
            Random rnd = new Random();
            return rnd.Next(10000, 99999);
        }
        public AgreementCreatedMessage AgreementCreated(long accountId)
        {
            return new AgreementCreatedMessage
            {
                AccountId = accountId,
                LegalEntityId = 8547,
                AgreementId = 9856
            };
        }

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
