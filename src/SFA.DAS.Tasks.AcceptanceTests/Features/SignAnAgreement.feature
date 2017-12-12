Feature: SignAnAgreement
	
@mytag
Scenario: Sign an Agreement
Given I Sign an agreement
When agreement_signed message get publish
Then I should have a AddApprentices Task
And AgreementToSign Task should be removed

Scenario: Create Draft Cohort
Given I Create Draft Cohort
When cohort_created message get publish
Then AddApprentices Task should be removed
