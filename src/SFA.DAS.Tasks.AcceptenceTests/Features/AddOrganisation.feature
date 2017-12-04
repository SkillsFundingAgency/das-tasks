Feature: AddOrganisation
AS AN employer 
I WANT a task to direct me 
SO THAT I can do my admin work efficiently 


Scenario: Sign an Agreement 
Given I Create an Account or add an Organisation
When agreement_created message get publish
Then I should have a AgreementToSign Task

