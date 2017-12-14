using TechTalk.SpecFlow;
using BoDi;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Commitments.Events;
using System.Collections.Generic;

namespace SFA.DAS.Tasks.AcceptanceTests.Steps
{
    [Binding]
    public class Arrange : TechTalk.SpecFlow.Steps
    {
        private IObjectContainer _objectContainer;
        private TestMessages _testMessages;

        public Arrange(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
            _testMessages = _objectContainer.Resolve<TestMessages>();
        }

        [Given(@"I Create an Account or add an Organisation")]
        public void GivenICreateAnAccountOrAddAnOrganisation()
        {
            var id = _objectContainer.Resolve<Dictionary<string, object>>("dictionary");
            _objectContainer.RegisterInstanceAs(_testMessages.AgreementCreated((long)id["employerAccountId"]));
        }

        [Given(@"I Sign an agreement")]
        public void GivenISignAnAgreement()
        {
            Given(@"I Create an Account or add an Organisation");
            When(@"agreement_created message get publish");
            var agreementcreated = _objectContainer.Resolve<AgreementCreatedMessage>();
            var agreementsigned = _testMessages.AgreementSigned(agreementcreated.AccountId, agreementcreated.LegalEntityId, agreementcreated.AgreementId);
            _objectContainer.RegisterInstanceAs(agreementsigned);
        }

        [Given(@"I add another Organisation and Remove an Organisation")]
        public void GivenIAddAnotherOrganisationAndRemoveAnOrganisation()
        {
            Given(@"I Create an Account or add an Organisation");
            When(@"agreement_created message get publish");
            When(@"agreement_created message get publish");
            var agreementcreated = _objectContainer.Resolve<AgreementCreatedMessage>();
            var legalEntityRemoved = _testMessages.LegalEntityRemovedMessage(agreementcreated.AccountId, agreementcreated.LegalEntityId, agreementcreated.AgreementId, false);
            _objectContainer.RegisterInstanceAs(legalEntityRemoved);
        }

        [Given(@"I Create Draft Cohort")]
        public void GivenICreateDraftCohort()
        {
            Given(@"I Sign an agreement");
            When(@"agreement_signed message get publish");
            var agreementsigned = _objectContainer.Resolve<AgreementSignedMessage>();
            var cohortCreated = _testMessages.CohortCreated(agreementsigned.AccountId);
            _objectContainer.RegisterInstanceAs(cohortCreated);
        }

        [Given(@"I have Draft Cohort")]
        public void GivenIHaveDraftCohort()
        {
            Given(@"I Create Draft Cohort");
            When(@"cohort_created message get publish");
            When(@"agreement_created message get publish");
            var agreementcreated = _objectContainer.Resolve<AgreementCreatedMessage>();
            var agreementsigned = _testMessages.AgreementSigned(agreementcreated.AccountId, agreementcreated.LegalEntityId, agreementcreated.AgreementId, true);
            _objectContainer.RegisterInstanceAs(agreementsigned, "cohortcreated=true");
        }

        [Given(@"I have an Apprenticeship Changes To Review")]
        public void GivenIHaveAnApprenticeshipChangesToReview()
        {
            Given(@"I Create Draft Cohort");
            When(@"cohort_created message get publish");
            var cohortCreated = _objectContainer.Resolve<CohortCreated>();
            var apprenticeshipchange = _testMessages.ApprenticeshipUpdateCreated(cohortCreated.AccountId, cohortCreated.ProviderId ?? 10000254);
            _objectContainer.RegisterInstanceAs(apprenticeshipchange);
        }

        [Given(@"I have an Apprenticeship Accepted")]
        public void GivenIHaveAnApprenticeshipAccepted()
        {
            Given(@"I have an Apprenticeship Changes To Review");
            When(@"apprenticeship_update_created message get publish");
            var apprenticeshipchange = _objectContainer.Resolve<ApprenticeshipUpdateCreated>();
            var apprenticeshipaccepted = _testMessages.ApprenticeshipUpdateAccepted(apprenticeshipchange.AccountId, apprenticeshipchange.ProviderId, apprenticeshipchange.ApprenticeshipId);
            _objectContainer.RegisterInstanceAs(apprenticeshipaccepted);
        }

        [Given(@"I have an Apprenticeship Rejected")]
        public void GivenIHaveAnApprenticeshipRejected()
        {
            Given(@"I have an Apprenticeship Changes To Review");
            When(@"apprenticeship_update_created message get publish");
            var apprenticeshipchange = _objectContainer.Resolve<ApprenticeshipUpdateCreated>();
            var apprenticeshiprejected = _testMessages.ApprenticeshipUpdateRejected(apprenticeshipchange.AccountId, apprenticeshipchange.ProviderId, apprenticeshipchange.ApprenticeshipId);
            _objectContainer.RegisterInstanceAs(apprenticeshiprejected);
        }

        [Given(@"I have an Apprenticeship Cancelled")]
        public void GivenIHaveAnApprenticeshipCancelled()
        {
            Given(@"I have an Apprenticeship Changes To Review");
            When(@"apprenticeship_update_created message get publish");
            var apprenticeshipchange = _objectContainer.Resolve<ApprenticeshipUpdateCreated>();
            var apprenticeshipCancelled = _testMessages.ApprenticeshipUpdateCancelled(apprenticeshipchange.AccountId, apprenticeshipchange.ProviderId, apprenticeshipchange.ApprenticeshipId);
            _objectContainer.RegisterInstanceAs(apprenticeshipCancelled);
        }

        [Given(@"I have Cohort Ready For Approval")]
        public void GivenIHaveCohortReadyForApproval()
        {
            Given(@"I Create Draft Cohort");
            var cohortcreated = _objectContainer.Resolve<CohortCreated>();
            var cohortApprovalRequestedByProvider = _testMessages.CohortApprovalRequestedByProvider(cohortcreated.AccountId, cohortcreated.CommitmentId, cohortcreated.ProviderId ?? 10000254);
            _objectContainer.RegisterInstanceAs(cohortApprovalRequestedByProvider);
        }

        [Given(@"I have Approved A Cohort")]
        public void GivenIHaveApprovedACohort()
        {
            Given(@"I have Cohort Ready For Approval");
            When(@"cohort_approval_requested_by_provider message get publish");
            var cohortApprovalRequestedByProvider = _objectContainer.Resolve<CohortApprovalRequestedByProvider>();
            var cohortApprovedByEmployer = _testMessages.CohortApprovedByEmployer(cohortApprovalRequestedByProvider.AccountId, cohortApprovalRequestedByProvider.CommitmentId, cohortApprovalRequestedByProvider.ProviderId);
            _objectContainer.RegisterInstanceAs(cohortApprovedByEmployer);
        }
    }
}
