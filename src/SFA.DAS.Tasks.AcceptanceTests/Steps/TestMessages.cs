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

        public string TestUserName => "Test User";

        public string TestUserRef => "ABC123";

        public string TestOrgainsationName => "Test Corp";

        private long GenerateRandomid()
        {
            Random rnd = new Random();
            return rnd.Next(10000, 99999);
        }
        public AgreementCreatedMessage AgreementCreated(long accountId)
        {
            return new AgreementCreatedMessage(accountId, 9856, TestOrgainsationName, 8547, TestUserName, TestUserRef);
        }

        public AgreementSignedMessage AgreementSigned(long accountId, long legalEntityId, long agreementId, bool cohortCreated = false)
        {
            return new AgreementSignedMessage(accountId, agreementId, legalEntityId, cohortCreated, TestUserName,
                TestUserRef);
        }

        public LegalEntityRemovedMessage LegalEntityRemovedMessage(long accountId, long legalEntityId, string legalEntityName, long agreementId,bool agreementSigned)
        {
            return new LegalEntityRemovedMessage(accountId, agreementId, agreementSigned, legalEntityId, 
                legalEntityName, TestUserName, TestUserRef);
        }

        public CohortCreated CohortCreated(long accountId)
        {
            long providerid = 100223547;
            long commitmentid = 65457812;
            return new CohortCreated(accountId, providerid, commitmentid);
        }

        public CohortApprovalRequestedByProvider CohortApprovalRequestedByProvider(long accountId, long commitmentid, long providerid)
        {
            return new CohortApprovalRequestedByProvider
            {
                AccountId = accountId,
                CommitmentId = commitmentid,
                ProviderId = providerid
            };
        }
        public CohortApprovedByEmployer CohortApprovedByEmployer(long accountId, long commitmentid, long providerid)
        {
            return new CohortApprovedByEmployer
            {
                AccountId = accountId,
                CommitmentId = commitmentid,
                ProviderId = providerid
            };
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
