using TechTalk.SpecFlow;
using BoDi;
using SFA.DAS.EmployerAccounts.Events.Messages;

namespace SFA.DAS.Tasks.AcceptenceTests.Steps
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
            _objectContainer.RegisterInstanceAs(_testMessages.AgreementCreated);
            _testMessages.NoofAgreementCreated++;
        }

        [Given(@"I Sign an agreement")]
        public void GivenISignAnAgreement()
        {
            Given(@"I Create an Account or add an Organisation");
            When(@"agreement_created message get publish");
            var agreementcreated = _objectContainer.Resolve<AgreementCreatedMessage>();
            var agreementsigned = _testMessages.AgreementSigned(agreementcreated.AccountId, agreementcreated.LegalEntityId, agreementcreated.AgreementId);
            _objectContainer.RegisterInstanceAs(agreementsigned);
            _testMessages.NoofAgreementSigned++;
        }

        [Given(@"I add another Organisation and Remove an Organisation")]
        public void GivenIAddAnotherOrganisationAndRemoveAnOrganisation()
        {
            Given(@"I Create an Account or add an Organisation");
            When(@"agreement_created message get publish");
            _testMessages.NoofAgreementCreated++;
            When(@"agreement_created message get publish");
            var agreementcreated = _objectContainer.Resolve<AgreementCreatedMessage>();
            var legalEntityRemoved = _testMessages.LegalEntityRemovedMessage(agreementcreated.AccountId, agreementcreated.LegalEntityId, agreementcreated.AgreementId, false);
            _objectContainer.RegisterInstanceAs(legalEntityRemoved);
        }
    }
}
