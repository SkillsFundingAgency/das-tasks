@AML1913
Feature: Transfers
	

Scenario: 040 Sent Transfer Connection Invitation
Given I have Sent Transfer Connection Invitation
When transfer_connection_invitation_sent message get publish
Then I should have a ReviewConnectionRequest Task
