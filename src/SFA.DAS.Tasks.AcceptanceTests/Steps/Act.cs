using System.Threading.Tasks;
using BoDi;
using SFA.DAS.Commitments.Events;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Tasks.AcceptanceTests.Commitments;
using SFA.DAS.Tasks.AcceptanceTests.DependencyResolution;
using SFA.DAS.Tasks.AcceptanceTests.EmployerAccounts;
using TechTalk.SpecFlow;
using ApprovedTransferConnectionInvitationEvent = SFA.DAS.EmployerFinance.Events.Messages.ApprovedTransferConnectionInvitationEvent;
using RejectedTransferConnectionInvitationEvent = SFA.DAS.EmployerFinance.Events.Messages.RejectedTransferConnectionInvitationEvent;
using SentTransferConnectionInvitationEvent = SFA.DAS.EmployerFinance.Events.Messages.SentTransferConnectionInvitationEvent;

namespace SFA.DAS.Tasks.AcceptanceTests.Steps
{
    [Binding]
    public class Act
    {
        private IObjectContainer _objectContainer;
        private IAzureTopicMessageBus _azureTopicMessageBus;
        private TestMessages _testMessages;

        public Act(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
            _testMessages = _objectContainer.Resolve<TestMessages>();
            _azureTopicMessageBus = _objectContainer.Resolve<IAzureTopicMessageBus>();
        }

        [When(@"(.*) bad message get publish by EAS")]
        public async Task WhenBadMessageGetPublishbyEAS(string message)
        {
            _objectContainer.RegisterInstanceAs(new EasBadTestMessage(), message);
            await PublishAndPeak<EasBadTestMessage>(message);
        }
        [When(@"(.*) bad message get publish by Commitments")]
        public async Task WhenBadMessageGetPublishByCommitments(string message)
        {
             _objectContainer.RegisterInstanceAs(new CommitmentsBadTestMessage(), message);
             await PublishAndPeak<CommitmentsBadTestMessage>(message);
        }

        [When(@"(agreement_created|agreement_signed|agreement_signed_cohort_created|legal_entity_removed|cohort_created) message get publish")]
        public async Task WhenAgreement_CreatedMessageGetPublish(string message)
        {
            switch (message)
            {
                case "agreement_created":
                    await PublishAndPeak<AgreementCreatedMessage>();
                    _testMessages.NoofAgreementCreated++;
                    break;
                case "agreement_signed":
                    await PublishAndPeak<AgreementSignedMessage>();
                    _testMessages.NoofAgreementSigned++;
                    break;
                case "agreement_signed_cohort_created":
                    await PublishAndPeak<AgreementSignedMessage>("cohortcreated=true");
                    _testMessages.NoofAgreementSigned++;
                    break;
                case "legal_entity_removed":
                    await PublishAndPeak<LegalEntityRemovedMessage>();
                    break;
                case "cohort_created":
                    await PublishAndPeak<CohortCreated>();
                    break;
            }
        }

        [When(@"(apprenticeship_update_created|apprenticeship_update_accepted|apprenticeship_update_rejected|apprenticeship_update_cancelled) message get publish")]
        public async Task WhenApprenticeship_CreatedMessageGetPublish(string message)
        {
            switch (message)
            {
                case "apprenticeship_update_created":
                    await PublishAndPeak<ApprenticeshipUpdateCreated>();
                    break;
                case "apprenticeship_update_accepted":
                    await PublishAndPeak<ApprenticeshipUpdateAccepted>();
                    break;
                case "apprenticeship_update_rejected":
                    await PublishAndPeak<ApprenticeshipUpdateRejected>();
                    break;
                case "apprenticeship_update_cancelled":
                    await PublishAndPeak<ApprenticeshipUpdateCancelled>();
                    break;
            }
        }

        [When(@"There are (3) (cohort_approval_requested_by_provider) message get publish")]
        public async Task WhenThereAreCohort_Approval_Requested_By_ProviderMessageGetPublish(int noOfTimes, string message)
        {
            for(int i=0; i<noOfTimes;i++)
            {
                await WhenCohort_Approval_Requested_By_ProviderMessageGetPublish(message);
            }
        }



        [When(@"(cohort_approval_requested_by_provider|cohort_approved_by_employer|provider_cohort_approval_undone_by_employer) message get publish")]
        public async Task WhenCohort_Approval_Requested_By_ProviderMessageGetPublish(string message)
        {
            switch (message)
            {
                case "cohort_approval_requested_by_provider":
                    await PublishAndPeak<CohortApprovalRequestedByProvider>();
                    break;
                case "cohort_approved_by_employer":
                    await PublishAndPeak<CohortApprovedByEmployer>();
                    break;
                case "provider_cohort_approval_undone_by_employer":
                    await PublishAndPeak<ProviderCohortApprovalUndoneByEmployerUpdate>();
                    break;
            }
        }

        [When(@"(transfer_connection_invitation_sent|approved_transfer_connection_invitation|rejected_transfer_connection_invitation) message get publish")]
        public async Task WhenTransfer_Connection_Invitation_MessageGetsPublished(string message)
        {
            switch (message)
            {
                case "transfer_connection_invitation_sent":
                    await PublishAndPeak<SentTransferConnectionInvitationEvent>();
                    break;

                case "approved_transfer_connection_invitation":
                    await PublishAndPeak<ApprovedTransferConnectionInvitationEvent>();
                    break;

                case "rejected_transfer_connection_invitation":
                    await PublishAndPeak<RejectedTransferConnectionInvitationEvent>();
                    break;
            }
        }

        private async Task PublishAndPeak<T>(string name = null)
        {
            var agreement = _objectContainer.Resolve<T>(name);

            if ((name == null) || (name != null && name == "cohortcreated=true"))
            {
                await _azureTopicMessageBus.PublishAsync(agreement);
                await Task.Delay(10000);
            }
            else
            {
                await _azureTopicMessageBus.PublishAsync(agreement, name);
            }

            //int count = 0;
            //while (true)
            //{
            //    var messageProcessed = await _azureTopicMessageBus.PeekAsync(agreement);

            //    if (messageProcessed != null)
            //    {
            //        System.Console.WriteLine($"Found {messageProcessed.ToString()}");
            //        await Task.Delay(5000);
            //    }

            //    if (messageProcessed == null || count >= 10)
            //    {
            //        break;
            //    }
            //    count++;
            //}
        }
    }
}

