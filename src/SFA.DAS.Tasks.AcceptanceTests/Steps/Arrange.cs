using TechTalk.SpecFlow;
using BoDi;
using SFA.DAS.EmployerAccounts.Events.Messages;
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
    }
}
