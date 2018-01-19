Feature: SignAnAgreement

@AML1239	
Scenario: 010 Sign an Agreement
Given I Sign an agreement
When agreement_signed message get publish
Then I should have a AddApprentices Task
And AgreementToSign Task should be removed

@AML1415
Scenario: 011 Create Draft Cohort
Given I Create Draft Cohort
When cohort_created message get publish
Then AddApprentices Task should be removed

@AML1415
Scenario: 012 Create Draft Cohort And Sign An Agreement
Given I have Draft Cohort
When agreement_signed_cohort_created message get publish
Then AddApprentices Task should not be added

Scenario: 013 agreement_signed Should Handle Bad Messsages
When agreement_created bad message get publish by EAS
Given I Sign an agreement
When agreement_signed message get publish
Then I should have a AddApprentices Task
And AgreementToSign Task should be removed