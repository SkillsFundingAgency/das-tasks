@AML1412
Feature: CohortApproved


Scenario: 030 Cohort Approval Requested By Provider
Given I have Cohort Ready For Approval
When cohort_approval_requested_by_provider message get publish
Then I should have a CohortRequestReadyForApproval Task


Scenario: 031 Cohort Approved By Employer
Given I have Approved A Cohort
When cohort_approved_by_employer message get publish
Then CohortRequestReadyForApproval Task should be removed


Scenario: 032 No Of Cohort Approval Requested By Provider
Given I have Cohort Ready For Approval
When There are 3 cohort_approval_requested_by_provider message get publish
Then I should have a 3 CohortRequestReadyForApproval Task


Scenario: 033 No Of Pending Cohort Approval By Employer
Given I have 3 Cohorts to Approve
When cohort_approved_by_employer message get publish
Then I should have a 2 CohortRequestReadyForApproval Task

Scenario: 034 Cohort Approval Undone By Employer
Given I have updated Cohort Approved By Provider
When provider_cohort_approval_undone_by_employer message get publish
Then CohortRequestReadyForApproval Task should be removed

Scenario: 035 Cohort Approval Requested By Provider Should Handle Bad Message
When cohort_approval_requested_by_provider bad message get publish by Commitments
Given I have Cohort Ready For Approval
When cohort_approval_requested_by_provider message get publish
Then I should have a CohortRequestReadyForApproval Task