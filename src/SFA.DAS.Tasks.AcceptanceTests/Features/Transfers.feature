@AML1913
Feature: Transfers
	

Scenario: 040 Sent Transfer Connection Invitation
Given I have Sent Transfer Connection Invitation
When transfer_connection_invitation_sent message get publish
Then I should have a ReviewConnectionRequest Task


Scenario: 041 Accept Transfer Connection Invitation
Given I have Sent Transfer Connection Invitation
When transfer_connection_invitation_sent message get publish
Then I should have a ReviewConnectionRequest Task
Given I have Approved A Transfer Connection Invitation
When approved_transfer_connection_invitation message get publish
Then I should not have a ReviewConnectionRequest Task

Scenario: 042 Reject Transfer Connection Invitation
Given I have Sent Transfer Connection Invitation
When transfer_connection_invitation_sent message get publish
Then I should have a ReviewConnectionRequest Task
Given I have Rejected A Transfer Connection Invitation
When rejected_transfer_connection_invitation message get publish
Then I should not have a ReviewConnectionRequest Task