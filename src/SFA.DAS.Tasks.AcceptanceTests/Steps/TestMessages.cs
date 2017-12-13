using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Commitments.Events;
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

        public AgreementSignedMessage AgreementSigned(long accountId, long legalEntityId, long agreementId, bool cohortCreated = false)
        {
            return new AgreementSignedMessage
            {
                AccountId = accountId,
                LegalEntityId = legalEntityId,
                AgreementId = agreementId,
                CohortCreated = cohortCreated
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

        public CohortCreated CohortCreated(long accountId)
        {
            long providerid = 100223547;
            long commitmentid = 65457812;
            return new CohortCreated(accountId, providerid, commitmentid);
        }

        public ApprenticeshipUpdateCreated ApprenticeshipUpdateCreated(long accountId, long providerid)
        {
            return new ApprenticeshipUpdateCreated
            {
                AccountId = accountId,
                ApprenticeshipId = 65457812,
                ProviderId = providerid
            };
        }
        public ApprenticeshipUpdateAccepted ApprenticeshipUpdateAccepted(long accountId, long providerid, long apprenticeshipId)
        {
            return new ApprenticeshipUpdateAccepted
            {
                AccountId = accountId,
                ApprenticeshipId = apprenticeshipId,
                ProviderId = providerid
            };
        }
        public ApprenticeshipUpdateRejected ApprenticeshipUpdateRejected(long accountId, long providerid, long apprenticeshipId)
        {
            return new ApprenticeshipUpdateRejected
            {
                AccountId = accountId,
                ApprenticeshipId = apprenticeshipId,
                ProviderId = providerid
            };
        }
        public ApprenticeshipUpdateCancelled ApprenticeshipUpdateCancelled(long accountId, long providerid, long apprenticeshipId)
        {
            return new ApprenticeshipUpdateCancelled
            {
                AccountId = accountId,
                ApprenticeshipId = apprenticeshipId,
                ProviderId = providerid
            };
        }
    }
}
