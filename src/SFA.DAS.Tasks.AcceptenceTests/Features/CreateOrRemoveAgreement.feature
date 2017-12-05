Feature: CreateOrRemoveAgreement
AS AN employer 
I WANT a task to direct me 
SO THAT I can do my admin work efficiently 


Scenario: Create Account Or Add an Organisation 
Given I Create an Account or add an Organisation
When agreement_created message get publish
Then I should have a AgreementToSign Task

Scenario: Remove an Organisation 
Given I add another Organisation and Remove an Organisation
When legal_entity_removed message get publish
Then AgreementToSign Task should be removed
